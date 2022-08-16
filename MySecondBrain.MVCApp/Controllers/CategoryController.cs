using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace MySecondBrain.MVCApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: DossierController
        public ActionResult Index(int? page)
        {
            var dossiers = Application.Services.DossierControllerService.GetAllDossiers(this.User);
            Application.ViewModels.CategoryListViewModel vm = new();
            vm.ListDossiers = dossiers.ListDossiers;
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Notes;
            vm.Notes = notes;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            vm.Tags = tags.Tags;
            return View(vm);
        }

        // GET: DossierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DossierController/POSTCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult POSTCreate(Application.ViewModels.CategoryDetailViewModel categoryDetailViewModel)
        {
            try
            {
                categoryDetailViewModel.Dossier.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Application.Services.DossierControllerService.CreateCategory(categoryDetailViewModel.Dossier);
                Application.ViewModels.CategoryListViewModel categoryListViewModel = Application.Services.DossierControllerService.GetAllDossiers(this.User);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DossierController/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = Application.Services.DossierControllerService.GetCategoryDetails(id);
            return View(vm);
        }

        // POST: DossierController/POSTEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult POSTEdit(Application.ViewModels.CategoryDetailViewModel categoryDetailViewModel)
        {
            try
            {
                Application.Services.DossierControllerService.EditCategory(categoryDetailViewModel.Dossier);
                Application.ViewModels.CategoryListViewModel vm = new();
                vm = Application.Services.DossierControllerService.GetAllListDossiers(this.User);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DossierController/Delete/5
        public ActionResult Delete(int id)
        {
            Application.Services.DossierControllerService.DeleteCategory(id);
            Application.ViewModels.CategoryListViewModel vm = new();
            vm = Application.Services.DossierControllerService.GetAllListDossiers(this.User);
            return RedirectToAction(nameof(Index));
        }

        // POST: DossierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
