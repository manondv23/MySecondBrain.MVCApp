using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MySecondBrain.Application.Services
{
    public class DossierControllerService
    {
        /// <summary>
        /// Appelle le service de la couche domain pour avoir tous les dossiers d'un user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public static ViewModels.NoteDetailViewModel GetListDossiers(ClaimsPrincipal user)
        {
        
            var dossiers = Domain.Services.CategoryService.GetAllDossiers(user);

            ViewModels.NoteDetailViewModel vm = new ViewModels.NoteDetailViewModel()
            {
                ListDossiers = dossiers
            };

        return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir tous les dossiers d'un user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public static ViewModels.CategoryListViewModel GetAllListDossiers(ClaimsPrincipal user)
        {

            var dossiers = Domain.Services.CategoryService.GetAllDossiers(user);

            ViewModels.CategoryListViewModel vm = new ViewModels.CategoryListViewModel()
            {
                ListDossiers = dossiers
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir le nom d'une catégorie en fonction de son id
        /// </summary>
        /// <param name="dossierId">Id du dossier</param>
        /// <returns></returns>
        public static ViewModels.NoteDetailViewModel GetDossierName(int dossierId)
        {

            var dossier = Domain.Services.CategoryService.GetDossierName(dossierId);

            ViewModels.NoteDetailViewModel vm = new ViewModels.NoteDetailViewModel()
            {
                newDossierName = dossier
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir tous les dossiers du user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public static ViewModels.CategoryListViewModel GetAllDossiers(ClaimsPrincipal user)
        {

            var dossiers = Domain.Services.CategoryService.GetAllDossiers(user);

            ViewModels.CategoryListViewModel vm = new ViewModels.CategoryListViewModel()
            {
                ListDossiers = dossiers
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste de toutes les catégories (noms à afficher dans la page dashboard)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public static ViewModels.NoteListViewModel GetListDossiersDashboard(ClaimsPrincipal user)
        {

            var dossiers = Domain.Services.CategoryService.GetAllDossiers(user);

            ViewModels.NoteListViewModel vm = new ViewModels.NoteListViewModel()
            {
                ListDossiers = dossiers
            };

            return vm;
        }

        /// <summary>
        /// Appelle le service de la couche domain pour renvoyer tous les détails de la catégorie
        /// </summary>
        /// <param name="catId">Id de la catégorie à renvoyer</param>
        /// <returns></returns>
        public static ViewModels.CategoryDetailViewModel GetCategoryDetails(int catId)
        {
            Infrastructure.DB.Dossier category = Domain.Services.CategoryService.GetCategory(catId);
            ViewModels.CategoryDetailViewModel vm = new()
            {
                Dossier = category
            };

            return vm;

        }

        /// <summary>
        /// Appelle le service de la couche domain pour créer un catégorie
        /// </summary>
        /// <param name="dossier">Dossier/Catégorie à créer</param>
        /// <returns></returns>
        public static void CreateCategory(Infrastructure.DB.Dossier dossier)
        {
            Domain.Services.CategoryService.CreateCategory(dossier);
        }

        /// <summary>
        /// Appelle le service de la couche domain pour éditer une note
        /// </summary>
        /// <param name="noteId">Note à éditer</param>
        /// <returns></returns>
        public static void EditCategory(Infrastructure.DB.Dossier dossier)
        {
            Domain.Services.CategoryService.EditCategory(dossier);
        }

        /// <summary>
        /// Supprime une cat
        /// </summary>
        /// <param name="categoryId">cat a supprimer</param>
        public static void DeleteCategory(int catId)
        {
            Domain.Services.CategoryService.DeleteCategory(catId);
        }
    }
}
