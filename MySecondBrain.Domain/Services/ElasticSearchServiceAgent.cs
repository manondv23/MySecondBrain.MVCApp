using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace MySecondBrain.Domain.Services.ElasticSearch
{
    /// <summary>
    /// Agent de service responsable d'encapsuler les accès à ElasticSearch
    /// </summary>
    public class ElasticSearchServiceAgent
    {
        // à déplacer dans l'appsettings
        static string elasticAddress = "http://localhost:9200";
        const string noteIndexName = "note_index";
        const string tagIndexName = "tag_index";

        /// <summary>
        /// Crée l'objet settings nécessaire pour se connecter à ES
        /// </summary>
        /// <returns></returns>
        static ConnectionSettings GetESConnectionSettings()
        {
            var node = new Uri(elasticAddress);
            var settings = new ConnectionSettings(node);
            settings.ThrowExceptions(alwaysThrow: true);
            settings.PrettyJson(); // Good for DEBUG

            return settings;
        }

        /// <summary>
        ///  Création des index dans ElasticSearch
        /// </summary>
        public static bool CreateIndexes()
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            if (esClient.Indices.Exists(noteIndexName).Exists)
            {
                var response = esClient.Indices.Delete(noteIndexName);
            }

            if (esClient.Indices.Exists(tagIndexName).Exists)
            {
                var response = esClient.Indices.Delete(tagIndexName);
            }

            var createIndexResponse = esClient.Indices.Create(noteIndexName, index => index.Map<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(x => x.AutoMap()));
            var createIndexResponse2 = esClient.Indices.Create(tagIndexName, index => index.Map<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(x => x.AutoMap()));
            
            
            return createIndexResponse.IsValid && createIndexResponse2.IsValid;
        }

        public static bool IndexAllNotes(List<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> noteDocuments)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            foreach (var noteDocument in noteDocuments)
            {
                var indexResponse = esClient.Index(noteDocument, c => c.Index(noteIndexName));

                if (!indexResponse.IsValid)
                    return false;
            }

            return true;
        }

        public static bool IndexAllTags(List<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> tagDocuments)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            foreach (var tagDocument in tagDocuments)
            {
                var indexResponse = esClient.Index(tagDocument, c => c.Index(tagIndexName));

                if (!indexResponse.IsValid)
                    return false;
            }

            return true;
        }

        public static bool IndexNote(Infrastructure.ElasticSearch.IndexDocuments.NoteDocument noteDocument)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            var indexResponse = esClient.Index(noteDocument, c => c.Index(noteIndexName));

            return indexResponse.IsValid;
        }

        public static bool IndexTag(Infrastructure.ElasticSearch.IndexDocuments.TagDocument tagDocument)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            var indexResponse = esClient.Index(tagDocument, c => c.Index(tagIndexName));

            return indexResponse.IsValid;
        }

        /// <summary>
        /// Renvoie toutes les notes indexées
        /// </summary>
        /// <returns>La liste des notes</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> GetAllNotes()
        {
            var esClient = new ElasticClient(GetESConnectionSettings());
             
            // récupération de tous les documents de l'index des albums
            var notes = esClient.Search<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(search =>
                    search.Index(noteIndexName)
                            .Size(1000)
                            .Query(q => q.MatchAll()));


            return notes.Documents.ToList();
        }

        /// <summary>
        /// Renvoie tous les tags indexés
        /// </summary>
        /// <returns>La liste des tags</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> GetAllTags()
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            // récupération de tous les documents de l'index des albums
            var tags = esClient.Search<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(search =>
                    search.Index(tagIndexName)
                            .Size(1000)
                            .Query(q => q.MatchAll()));


            return tags.Documents.ToList();
        }

        /// <summary>
        /// Renvoie toutes les notes indexées d'un user
        /// </summary>
        /// <param name="userId">L'id du user</param>
        /// <returns>La liste des notes du user</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> GetAllNotesOfUser(string userId)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());
            // récupération de tous les documents de l'index des albums du user
            var notes = esClient.Search<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(search =>
                    search.Index(noteIndexName)
                            .Size(1000)
                            .Query(q => q.Match(m => m.Field(f => f.NoteUserId)
                                                     .Query(userId)))); 
            return notes.Documents.ToList().OrderBy(m => m.NoteDateCreation);
        }

        /// <summary>
        /// Renvoie toutes les tags indexés d'un user
        /// </summary>
        /// <param name="userId">L'id du user</param>
        /// <returns>La liste des tags du user</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> GetAllTagsOfUser(string userId)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());
            // récupération de tous les documents de l'index des albums du user
            var tags = esClient.Search<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(search =>
                    search.Index(tagIndexName)
                            .Size(1000)
                            .Query(q => q.Match(m => m.Field(f => f.TagUserId)
                                                     .Query(userId))));
            return tags.Documents.ToList();
        }

        /// <summary>
        /// Renvoie toutes les notes indexées liées à un tag
        /// </summary>
        /// <param name="listNotes">La liste des notes qui possèdent le tag</param>
        /// <returns>La liste des notes</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> GetAllNotesByTag(List<Infrastructure.DB.Note> listNotes)
        {
            var esClient = new ElasticClient(GetESConnectionSettings());

            List<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> noteDocuments = new();
            if (listNotes != null)
            {
                foreach (var item in listNotes)
                {
                    var noteDocument = GetAllNotes().Where(n => n.NoteId == item.Idnote).First();
                    noteDocuments.Add(noteDocument);
                }
            }
            return noteDocuments.ToList().OrderBy(m => m.NoteDateCreation);
        }

        /// <summary>
        /// Renvoie toutes les notes indexées d'un dossier
        /// </summary>
        /// <param name="dossierId">L'id du dossier</param>
        /// <returns>La liste des notes</returns>
        public static List<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> SearchNotesByDossier(int dossierId)
        {
            var client = new ElasticClient(GetESConnectionSettings());

            var notes = client.Search<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(search =>
                                search.Index(noteIndexName).Query(q => q.Term(t => t.NoteDossierId, dossierId)));

            return notes.Documents.ToList();
        }

        /// <summary>
        /// Renvoie la note indexée qui correspond à un id
        /// </summary>
        /// <param name="id">L'id de la note</param>
        /// <returns>La note</returns>
        public static Infrastructure.ElasticSearch.IndexDocuments.NoteDocument SearchNoteById(int id)
        {
            var client = new ElasticClient(GetESConnectionSettings());

            var notes = client.Search<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(search =>
                                search.Index(noteIndexName).Query(q => q.Term(t => t.NoteId, id)));

            return notes.Documents.SingleOrDefault();
        }

        /// <summary>
        /// Renvoie le tag indexé qui correspond à un id
        /// </summary>
        /// <param name="id">L'id du tag</param>
        /// <returns>Le tag</returns>
        public static Infrastructure.ElasticSearch.IndexDocuments.TagDocument SearchTagById(int id)
        {
            var client = new ElasticClient(GetESConnectionSettings());

            var tags = client.Search<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(search =>
                                search.Index(tagIndexName).Query(q => q.Term(t => t.TagId, id)));

            return tags.Documents.SingleOrDefault();
        }

        /// <summary>
        /// Renvoie les notes qui comprennent la query
        /// </summary>
        /// <param name="searchQuery">La query entrée dans la search bar</param>
        /// <returns>La liste des notes</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> SearchNotes(string searchQuery)
        {
            var client = new ElasticClient(GetESConnectionSettings());

            // ex: get all documents in index
            var notes = client.Search<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(search =>
                                search.Index(noteIndexName)
                                        .Size(1000)
                                        .Query(q => q.MultiMatch(m => m.Query(searchQuery))));

            return notes.Documents.ToList();


        }

        /// <summary>
        /// Renvoie les tags qui comprennent la query
        /// </summary>
        /// <param name="searchQuery">La query entrée dans la search bar</param>
        /// <returns>La liste des tags</returns>
        public static IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> SearchTags(string searchQuery)
        {
            var client = new ElasticClient(GetESConnectionSettings());

            // ex: get all documents in index
            var tags = client.Search<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(search =>
                                search.Index(tagIndexName)
                                        .Size(1000)
                                        .Query(q => q.MultiMatch(m => m.Query(searchQuery))));

            return tags.Documents.ToList();


        }

        /// <summary>
        /// Modifie une note dans les index
        /// </summary>
        /// <param name="note">La note à modifier</param>
        public static void EditNote(Infrastructure.DB.Note note)
        {
            var client = new ElasticClient(GetESConnectionSettings());
            var noteDocument = ElasticSearchServiceAgent.SearchNoteById(note.Idnote);
            client.Update<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>
               (noteDocument.NoteId,
               u => u.Index(noteIndexName)
                     .Doc(new Infrastructure.ElasticSearch.IndexDocuments.NoteDocument
                     {
                         NoteTitle = note.Title,
                         NoteDescription = note.Description,
                         NoteContenuMarkdown = note.ContenuMarkdown,
                         NoteDateCreation = (DateTime)note.DateCreation,
                         NoteDossierId = note.Iddossier,
                         NoteId = note.Idnote,
                       
                     })
               );

        }

        /// <summary>
        /// Modifie un tag dans les index
        /// </summary>
        /// <param name="tag">Le tag à modifier</param>
        public static void EditTag(Infrastructure.DB.Tag tag)
        {
            var client = new ElasticClient(GetESConnectionSettings());
            var tagDocument = ElasticSearchServiceAgent.SearchTagById(tag.Idtag);
            client.Update<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>
               (tagDocument.TagId,
               u => u.Index(tagIndexName)
                     .Doc(new Infrastructure.ElasticSearch.IndexDocuments.TagDocument
                     {
                         TagName = tag.Nom,
                         TagId = tag.Idtag,

                     })
               );

        }

        /// <summary>
        /// Supprime une note des index
        /// </summary>
        /// <param name="noteId">L'id de la note à supprimer</param>
        public static void RemoveNote(int noteId)
        {
            var client = new ElasticClient(GetESConnectionSettings());
            var response = client.Delete<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>(noteId, d => d.Index(noteIndexName));
        }

        /// <summary>
        /// Supprime un tag des index
        /// </summary>
        /// <param name="tagId">L'id du tag à supprimer</param>
        public static void RemoveTag(int tagId)
        {
            var client = new ElasticClient(GetESConnectionSettings());
            var response = client.Delete<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>(tagId, d => d.Index(tagIndexName));
        }

    }
}
 