using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySecondBrain.MVCApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            if (Domain.Services.ElasticSearch.ElasticSearchServiceAgent.CreateIndexes())
            {
                IndexDatabaseNote();
                IndexDatabaseTag();

            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public static void IndexDatabaseNote()
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

        public static void IndexDatabaseTag()
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
