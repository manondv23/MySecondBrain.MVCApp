using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace MySecondBrain.Domain.Services
{
    public class NoteService
    {
        /// <summary>
        /// Renvoie toutes les notes de la db
        /// </summary>
        /// <returns>Liste des notes</returns>
        public static List<Infrastructure.DB.Note> GetAllNotes()
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Notes.ToList();
            }
        }

        /// <summary>
        /// Renvoie les notes du user
        /// </summary>
        /// <param name="user">Le user</param>
        /// <returns>La liste des notes</returns>
        public static List<Infrastructure.DB.Note> GetAllNotesForUser(ClaimsPrincipal user)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Notes.Where(n => n.User.Email == user.Identity.Name).ToList();
            }
        }

        /// <summary>
        /// Renvoie la liste des notes du user qui possède le tag
        /// </summary>
        /// <param name="tagId">L'id du tag</param>
        /// <param name="userId">L'id du user</param>
        /// <returns>La liste des notes</returns>
        public static List<Infrastructure.DB.Note> GetAllNotesByTag(string userId, int tagId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())

            {
                
                var innerJoin = from n in db.Notes
                                join nt in db.NoteTags
                                on n.Idnote equals nt.Idnote
                                where nt.Idtag == tagId
                                where n.UserId == userId
                                select n;

                return innerJoin.ToList();
            }
        }


        /// <summary>
        /// Renvoie une note grace a son id
        /// </summary>
        /// <param name="noteId">L'id de la note</param>
        /// <returns>Note</returns>
        public static Infrastructure.DB.Note GetNote(int noteId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Notes.Find(noteId);
            }
        }
        /// <summary>
        /// Créer une note
        /// </summary>
        /// <param name="note">La note à créer</param>
        /// <param name="newCategoryName">La categorie à créer et ajouter si le user en a entré une</param>
        /// <param name="userId">L'id du user lié à la note</param>
        /// <param name="tags">La liste des tags à ajouter à cette note</param>
        /// <param name="newTagName">Le tag a ajouter et créer si le user en a entré un</param>
        public static void CreateNote(Infrastructure.DB.Note note, string newCategoryName, string userId, List<int> tags, string newTagName)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                note.DateCreation = DateTime.Now;
                if (newCategoryName != null && newCategoryName != "")
                {
                    Infrastructure.DB.Dossier newDossier = new()
                    {
                        Nom = newCategoryName,
                        UserId = note.UserId
                        
                    };
                    db.Dossiers.Add(newDossier);
                    db.SaveChanges();
                    note.Iddossier = newDossier.Iddossier;

                }
                db.Notes.Add(note);
                db.SaveChanges();
               
                var noteDocument = new Infrastructure.ElasticSearch.IndexDocuments.NoteDocument()
                {
                    NoteId = note.Idnote,
                    NoteTitle = note.Title,
                    NoteDescription = note.Description,
                    NoteContenuMarkdown = note.ContenuMarkdown,
                    NoteDossierId = note.Iddossier,
                    NoteDateCreation = DateTime.Now,
                    NoteUserId = userId,
                };

                if (tags != null)
                {
                    foreach (var tagId in tags)
                    {
                        Infrastructure.DB.NoteTag noteTag = new();
                        noteTag.Idnote = note.Idnote;
                        noteTag.Idtag = tagId;
                        note.NoteTags.Add(noteTag);
                    }
                }

                if (newTagName != null && newTagName != "")
                {
                    Infrastructure.DB.Tag newTag = new()
                    {
                        Nom = newTagName,
                        UserId = note.UserId
                    };
                    db.Tags.Add(newTag);
                    db.SaveChanges();
                    Infrastructure.DB.NoteTag noteTag2 = new();
                    noteTag2.Idnote = note.Idnote;
                    noteTag2.Idtag = newTag.Idtag;
                    note.NoteTags.Add(noteTag2);
                    db.SaveChanges();
                    var tagDocument = new Infrastructure.ElasticSearch.IndexDocuments.TagDocument()
                    {
                        TagId = newTag.Idtag,
                        TagName = newTag.Nom,
                        TagUserId = userId,
                      
                    };
                    ElasticSearch.ElasticSearchServiceAgent.IndexTag(tagDocument);
                }
                db.SaveChanges();
                ElasticSearch.ElasticSearchServiceAgent.IndexNote(noteDocument);
            }
        }

        /// <summary>
        /// Modifie une note
        /// </summary>
        /// <param name="note">La note à modifier</param>
        public static void EditNote(Infrastructure.DB.Note note)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new())
            {
                db.Notes.Update(note);
                db.SaveChanges();
                Domain.Services.ElasticSearch.ElasticSearchServiceAgent.EditNote(note);
            }
        }

        /// <summary>
        /// Ajoute des tags à une note
        /// </summary>
        /// <param name="tagsId">Les ids des tags à ajouter</param>
        /// <param name="noteId">La note</param>
        public static void AddTagsToNote(List<int> tagsId, int noteId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                if (tagsId != null)
                {
                    foreach (var tagId in tagsId)
                    {
                        Infrastructure.DB.NoteTag noteTag = new();
                        noteTag.Idnote = noteId;
                        noteTag.Idtag = tagId;
                        db.NoteTags.Add(noteTag);
                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Supprime une note
        /// </summary>
        /// <param name="noteId">La note à supprimer</param>
        public static void DeleteNote(int noteId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                Infrastructure.DB.Note note = db.Notes.Find(noteId);
                if (note != null)
                {
                  
                    db.Notes.Remove(note);
                    db.NoteTags.Where(x => x.Idnote == noteId).ToList().ForEach(x => db.NoteTags.Remove(x));
                    db.SaveChanges();
                    ElasticSearch.ElasticSearchServiceAgent.RemoveNote(noteId);
                }
            }
        }

    }
}