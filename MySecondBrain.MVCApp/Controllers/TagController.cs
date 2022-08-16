using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace MySecondBrain.MVCApp.Controllers
{
    [Authorize]
    public class TagController : Controller
    {

        // GET: TagController
        public ActionResult Index(int? page)
        {
            Application.ViewModels.TagListViewModel model = new();
            model.Tags = Application.Services.TagsControllerService.GetAllTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Notes;
            model.Notes = notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            model.ListDossiers = dossiers;
            return View(model);
        }

        // POST: TagController/query/1
        [HttpPost]
        public ActionResult Index(string query)
        {
            Application.ViewModels.TagListViewModel vm = new();
            var tags = Application.Services.TagsControllerService.GetTagListFromQuery(query).Tags;
            vm.Tags = tags;
            vm.CustomTitle = "Tags with " + query;
            var dossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Notes;
            vm.Notes = notes;
            return View(vm);
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/POSTCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult POSTCreate(Application.ViewModels.TagDetailViewModel tagDetailViewModel)
        {
            try
            {
                tagDetailViewModel.Tag.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Application.Services.TagsControllerService.CreateTag(tagDetailViewModel.Tag);
                Application.ViewModels.TagListViewModel tagListViewModel = new();
                var tags = Application.Services.TagsControllerService.GetAllTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
                tagDetailViewModel.Tags = tags;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = Application.Services.TagsControllerService.GetTagDetails(id);
            return View(vm);
        }

        // POST: TagController/POSTEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult POSTEdit(Application.ViewModels.TagDetailViewModel tagDetailViewModel)
        {
            try
            {
                Application.Services.TagsControllerService.EditTag(tagDetailViewModel.Tag);
                Application.ViewModels.TagListViewModel vm = new();
                vm.Tags = Application.Services.TagsControllerService.GetAllTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            Application.Services.TagsControllerService.DeleteTag(id);
            Application.ViewModels.TagListViewModel vm = new();
            vm.Tags = Application.Services.TagsControllerService.GetAllTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
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
