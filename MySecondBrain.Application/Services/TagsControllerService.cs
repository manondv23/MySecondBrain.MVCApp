using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MySecondBrain.Application.Services
{
    public class TagsControllerService
    {
        /// <summary>
        /// Appelle le service de la couche domain pour avoir les tags du user
        /// </summary>
        /// <param name="currentTags">Tags déjà liés à la note</param>
        /// <param name="userId">L'id du user</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetTagsOfUser(string userId)
        {

            var tags = Domain.Services.TagService.GetAllTagsOfUser(userId);

            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel()
            {
                Tags = tags,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir les tags du user 
        /// </summary>
        /// <param name="userId">L'id du user</param>
        /// <returns></returns>
        public static ViewModels.TagListViewModel GetAllTagsOfUser(string userId)
        {

            var tags = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.GetAllTagsOfUser(userId);

            ViewModels.TagListViewModel vm = new ViewModels.TagListViewModel()
            {
                Tags = tags,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir les détails du tag
        /// </summary>
        /// <param name="tagId">Id du tag</param>
        /// <returns></returns>
        public static ViewModels.TagDetailViewModel GetTagDetails(int tagId)
        {
            Infrastructure.DB.Tag tag = Domain.Services.TagService.GetTag(tagId);
            ViewModels.TagDetailViewModel vm = new()
            {
                Tag = tag
            };

            return vm;

        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir les tags d'une note
        /// </summary>
        /// <param name="noteId">Id de la note</param>
        /// <returns></returns>
        public static ViewModels.NoteDetailViewModel GetTagsOfNote(int noteId)
        {

            var tags = Domain.Services.TagService.GetAllTagsOfNote(noteId);

            ViewModels.NoteDetailViewModel vm = new ViewModels.NoteDetailViewModel()
            {
                TagsOfNote = tags,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir les tags qui comprennent la query
        /// </summary>
        /// <param name="query">La query (mot) recherché</param>
        /// <returns></returns>
        public static ViewModels.TagListViewModel GetTagListFromQuery(string query)
        {
            ViewModels.TagListViewModel vm = new ViewModels.TagListViewModel();
            vm.Tags = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.SearchTags(query);

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir les tags du user SANS ceux qui sont déjà liés à la note
        /// </summary>
        /// <param name="currentTags">Tags déjà liés à la note</param>
        /// <param name="userId">L'id du user</param>
        /// <returns></returns>
        public static ViewModels.NoteDetailViewModel GetTagsWithoutCurrents(List<Infrastructure.DB.Tag> currentTags, string userId)
        {

            var tags = Domain.Services.TagService.GetTagsWithoutCurrents(currentTags, userId);

            ViewModels.NoteDetailViewModel vm = new ViewModels.NoteDetailViewModel()
            {
                Tags = tags,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour créer un tag
        /// </summary>
        /// <param name="tag">Tag à créer</param>
        /// <returns></returns>
        public static void CreateTag(Infrastructure.DB.Tag tag)
        {
            Domain.Services.TagService.CreateTag(tag);
        }

        /// <summary>
        /// Appelle le service de la couche domain pour éditer une note
        /// </summary>
        /// <param name="noteId">Note à éditer</param>
        /// <returns></returns>
        public static void EditTag(Infrastructure.DB.Tag tag)
        {
            Domain.Services.TagService.EditTag(tag);
        }

        /// <summary>
        /// Supprime une cat
        /// </summary>
        /// <param name="categoryId">cat a supprimer</param>
        public static void DeleteTag(int tagId)
        {
            Domain.Services.TagService.DeleteTag(tagId);
        }

        /// <summary>
        /// Supprime une cat
        /// </summary>
        /// <param name="categoryId">cat a supprimer</param>
        public static void DeleteNoteTag(int tagId, int noteId)
        {
            Domain.Services.TagService.DeleteNoteTag(tagId, noteId);
        }
    }
}
