using System;
using System.Collections.Generic;
using System.Linq;

namespace MySecondBrain.TestConsole
{
    class Program
    {
        /// <summary>
        /// Méthode principale qui appelle TestServices.
        /// </summary>
        /// <returns></returns>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

                      if (Domain.Services.ElasticSearch.ElasticSearchServiceAgent.CreateIndexes())
                        {
                            Console.WriteLine("Index créés avec succès :-)");
                            IndexDatabaseNote();
                            IndexDatabaseTag();
                
                        }
                        else
                            Console.WriteLine("Problème pendant la création des index!");

           /* var notesFound = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.SearchNotes("Latin");
            Console.WriteLine(notesFound);
            foreach (var note in notesFound)
            {
                Console.WriteLine(note.NoteTitle);
            }
            if (notesFound == null)
            {
                Console.WriteLine("pas cool");
            } */

            

            TestServices();
        
        }

        /// <summary>
        /// Test diverses opérations en DB via le projet Domain.
        /// </summary>
        /// <returns></returns>
        public static void TestServices()
        {
            TestTags();
            TestNotes();
        }

        /// <summary>
        /// Effectue des opérations (lecture, écriture) sur les tags.
        /// </summary>
        /// <returns></returns>
        public static void TestTags()
        {
            foreach (var item in Domain.Services.TagService.GetAllTags())
            {
                Console.WriteLine(item.Nom);
            }
        }

        /// <summary>
        /// Effectue des opérations (lecture, écriture) sur les Notes.
        /// </summary>
        /// <returns></returns>
        public static void TestNotes()
        {
          
            foreach (var item in Domain.Services.NoteService.GetAllNotes())
            {
                Console.WriteLine(item.Title);
            }
        }

        private static void IndexDatabaseNote()
        {
            var noteDocuments = new List<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument>();

            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                // on construit la liste des documents à indexer sur base du contenu de la DB
                foreach (var note in db.Notes.ToList())
                {
                    var noteDocument = new Infrastructure.ElasticSearch.IndexDocuments.NoteDocument()
                    {
                        NoteId = note.Idnote,
                        NoteTitle = note.Title,
                        NoteDescription = note.Description,
                        NoteContenuMarkdown = note.ContenuMarkdown,
                        NoteUserId = note.UserId,
                        NoteDossierId = note.Iddossier,
                        NoteDateCreation = DateTime.Now,

                    };

                    noteDocuments.Add(noteDocument);
                }
            }

            // on indexe
            if (Domain.Services.ElasticSearch.ElasticSearchServiceAgent.IndexAllNotes(noteDocuments))
                Console.WriteLine("Notes indexées avec succès :-)");
            else
                Console.WriteLine("Une erreur s'est produite pendant l'indexation des notes!");
        }

        private static void IndexDatabaseTag()
        {
            var tagDocuments = new List<Infrastructure.ElasticSearch.IndexDocuments.TagDocument>();

            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                // on construit la liste des documents à indexer sur base du contenu de la DB
                foreach (var tag in db.Tags.ToList())
                {
                    var tagDocument = new Infrastructure.ElasticSearch.IndexDocuments.TagDocument()
                    {
                        TagId = tag.Idtag,
                        TagName = tag.Nom,
                        TagUserId = tag.UserId,
                    };

                    tagDocuments.Add(tagDocument);
                }
            }

            // on indexe
            if (Domain.Services.ElasticSearch.ElasticSearchServiceAgent.IndexAllTags(tagDocuments))
                Console.WriteLine("Tags indexés avec succès :-)");
            else
                Console.WriteLine("Une erreur s'est produite pendant l'indexation des tags!");
        }

    }
}
