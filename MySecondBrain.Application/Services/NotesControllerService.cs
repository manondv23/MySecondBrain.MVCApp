using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.Services
{
    public class NotesControllerService
    {
        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des toutes les notes de la DB
        /// </summary>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetNoteList()
        {
            var notes = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.GetAllNotes();

            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel()
            {
                Notes = notes,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des notes d'un user
        /// </summary>
        /// <param name="userId">Id du user</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetNoteListOfUser(string userId)
        {

            var notes = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.GetAllNotesOfUser(userId);
            
            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel()
            {
                Notes = notes,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des notes d'un user qui comprenant un certain tag
        /// </summary>
        /// <param name="userId">Id du user</param>
        /// <param name="tagId">Id du tag</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetNoteListByTagId(string userId, int tagId)
        {

            var notes = Domain.Services.NoteService.GetAllNotesByTag(userId, tagId);

            var notesDocument = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.GetAllNotesByTag(notes);

            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel()
            {
                Notes = notesDocument,
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des notes en fonction d'une query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetNoteListFromQuery(string query)
        {
            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel();
            vm.Notes = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.SearchNotes(query);

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des notes en fonction d'un dossier/catégorie
        /// </summary>
        /// <param name="dossier">L'id du dossier</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetNoteListFromDossier(int dossier)
        {
            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel();
            vm.Notes = Domain.Services.ElasticSearch.ElasticSearchServiceAgent.SearchNotesByDossier(dossier);

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir le détail d'une note
        /// </summary>
        /// <param name="noteId">Id de la note</param>
        /// <returns></returns>
        public static ViewModels.NoteDetailViewModel GetNoteDetails(int noteId)
        {
            Infrastructure.DB.Note note = Domain.Services.NoteService.GetNote(noteId);
            ViewModels.NoteDetailViewModel vm = new()
            {
                Note = note
            };

            return vm;

        }

        /// <summary>
        /// Appelle le service de la couche domain pour ajouter des tags à une note
        /// </summary>
        /// <param name="tagsIdToAdd">Liste des tags à ajouter</param>
        /// <param name="noteId">Id de la note</param>
        /// <returns></returns>
        public static void AddTagsToNote(List<int> tagsIdToAdd, int noteId)
        {
            Domain.Services.NoteService.AddTagsToNote(tagsIdToAdd, noteId);

        }

        /// <summary>
        /// Appelle le service de la couche domain pour créer une note
        /// </summary>
        /// <param name="note">La note à créer</param>
        /// <param name="newCategoryName">Optionnel : le nom d'une nouvelle catégorie qui sera liée à la note</param>
        /// <param name="userId">L'id du user</param>
        /// <param name="newTagName">Optionnel : le nom d'un nouveau tag qui sera lié à la note</param>
        /// <param name="tags">La liste des tags cochés (déjà créés par le user) à ajouter à la note</param>
        /// <returns></returns>
        public static void CreateNote(Infrastructure.DB.Note note, string newCategoryName, string userId, List<int> tags, string newTagName)
        {
            Domain.Services.NoteService.CreateNote(note, newCategoryName, userId, tags, newTagName);
        }

        /// <summary>
        /// Appelle le service de la couche domain pour éditer une note
        /// </summary>
        /// <param name="noteId">Note à éditer</param>
        /// <returns></returns>
        public static void EditNote(Infrastructure.DB.Note note)
        {
            Domain.Services.NoteService.EditNote(note);
        }

        /// <summary>
        /// Supprime une note
        /// </summary>
        /// <param name="noteId">Note à éditer</param>
        public static void DeleteNote(int noteId)
        {
            Domain.Services.NoteService.DeleteNote(noteId);
        }
    }
}
