using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Domain.Services
{
    public class TagService
    {
        /// <summary>
        /// Renvoie les tags d'un user
        /// </summary>
        /// <returns>La liste de tags</returns>
        public static List<Infrastructure.DB.Tag> GetAllTags()
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Tags.ToList();
            }
        }

        /// <summary>
        /// Renvoie les tags d'un user
        /// </summary>
        /// <param name="userId">L'id du user</param>
        /// <returns>La liste de tags</returns>
        public static List<Infrastructure.DB.Tag> GetAllTagsOfUser(string userId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Tags.Where(n => n.UserId == userId).ToList();
            }
        }

        /// <summary>
        /// Renvoie tous les tags d'une note
        /// </summary>
        /// <param name="noteId">L'id de la note</param>
        /// <returns>La liste de tags</returns>
        public static List<Infrastructure.DB.Tag> GetAllTagsOfNote(int noteId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                
                var search = db.NoteTags.Where(n => n.Idnote == noteId).ToList();
                List<Infrastructure.DB.Tag> tags = new();
                foreach(var item in search)
                {
                    var tag = db.Tags.Find(item.Idtag);
                    tags.Add(tag);
                }
                return tags.ToList();
            }
        }

        /// <summary>
        /// Renvoie le tag en fonction de son id
        /// </summary>
        /// <param name="tagId">L'id du tag</param>
        /// <returns>Le tag</returns>
        public static Infrastructure.DB.Tag GetTag(int tagId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Tags.Find(tagId);
            }
        }

        /// <summary>
        /// Renvoie tous les tags du user SAUF ceux déjà liés à la note
        /// </summary>
        /// <param name="currentTags">Tags déjà liés à la note</param>
        /// <param name="userId">L'id du user</param>
        /// <returns>La liste de tags</returns>
        public static List<Infrastructure.DB.Tag> GetTagsWithoutCurrents(List<Infrastructure.DB.Tag> currentTags, string userId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {

                var allTags = GetAllTagsOfUser(userId).ToList();
                List<Infrastructure.DB.Tag> tagsResult = new();
                foreach (var item in allTags)
                {
                    if (!currentTags.Any(n => n.Idtag == item.Idtag))
                    {
                        tagsResult.Add(item);
                    };
                }
                return tagsResult.ToList();
            }
           
        }

        /// <summary>
        /// Créer un tag
        /// </summary>
        /// <param name="tag">Le tag</param>
        public static void CreateTag(Infrastructure.DB.Tag tag)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                db.Tags.Add(tag);
                db.SaveChanges();

                var tagDocument = new Infrastructure.ElasticSearch.IndexDocuments.TagDocument()
                {
                    TagId = tag.Idtag,
                    TagName = tag.Nom,
                    TagUserId = tag.UserId,
          
                };

                ElasticSearch.ElasticSearchServiceAgent.IndexTag(tagDocument);

            }
        }

        /// <summary>
        /// Modifie le tag
        /// </summary>
        /// <param name="tag">Le tag</param>
        public static void EditTag(Infrastructure.DB.Tag tag)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {

                db.Tags.Update(tag);
                db.SaveChanges();
                Domain.Services.ElasticSearch.ElasticSearchServiceAgent.EditTag(tag);

            }
        }

        /// <summary>
        /// Supprime le tag
        /// </summary>
        /// <param name="tagId">L'id du tag</param>
        public static void DeleteTag(int tagId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                Infrastructure.DB.Tag tag = db.Tags.Find(tagId);
                db.NoteTags.Where(x => x.Idtag == tagId).ToList().ForEach(x => db.NoteTags.Remove(x));
                db.Tags.Remove(tag);
                db.SaveChanges();
                
            }
        }

        /// <summary>
        /// Supprime la relation le tag de la note
        /// </summary>
        /// <param name="tagId">L'id du tag</param>
        /// <param name="noteId">L'id de la note</param>
        public static void DeleteNoteTag(int tagId, int noteId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                Infrastructure.DB.NoteTag noteTag = db.NoteTags.Find(noteId, tagId);
                db.NoteTags.Remove(noteTag);
                db.SaveChanges();
                ElasticSearch.ElasticSearchServiceAgent.RemoveTag(tagId);

            }
        }
    }
}
