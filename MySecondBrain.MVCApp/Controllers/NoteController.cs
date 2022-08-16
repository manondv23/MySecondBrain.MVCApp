using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MySecondBrain.MVCApp.Models;
using MySecondBrain.MVCApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using MySecondBrain.Application.ViewModels;
using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace MySecondBrain.Controllers
{

    [Authorize]
    public class NoteController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public NoteController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: NoteController
        public ActionResult Index(int? page)

        {
            Application.ViewModels.NoteListViewModel vm = new();
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Notes;
            vm.PaginatedNotes = new(notes, page ?? 0, 6);
            vm.Notes = notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            vm.Tags = tags.Tags;
            vm.CustomTitle = "";
            return View(vm);
        }

       // POST: NoteController/query/1
        [HttpPost]
        public ActionResult Index(string query, int? page)
        {
            Application.ViewModels.NoteListViewModel vm = new();
            var notes = Application.Services.NotesControllerService.GetNoteListFromQuery(query).Notes;
            vm.Notes = notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
            vm.Tags = tags;
            vm.PaginatedNotes = new(notes, page ?? 0, 6);
            vm.CustomTitle = "Notes with " + query;
            return View(vm);
        }

        // POST: NotreController/POSTDossierView/2/1
        [HttpPost]
        public ActionResult POSTDossierView(int dossier, int? page)
        {
            Application.ViewModels.NoteListViewModel vm = new();
            var notes = Application.Services.NotesControllerService.GetNoteListFromDossier(dossier).Notes;
            vm.Notes = notes;
            vm.ListDossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
            vm.Tags = tags;
            vm.PaginatedNotes = new(notes, page ?? 0, 6);
            var dossierName = Application.Services.DossierControllerService.GetDossierName(dossier);
            vm.CustomTitle = "In Category " + dossierName.newDossierName;
            return View("Index",vm);
        }

        // GET: NoteController/Detail/4
        public ActionResult Detail(int id)
        {
            var vm = Application.Services.NotesControllerService.GetNoteDetails(id);
            var tags = Application.Services.TagsControllerService.GetTagsOfNote(id).TagsOfNote;
            vm.Tags = tags;
            if (vm == null)
                return NotFound();

            return View(vm);
            
        }

        // GET: NoteController/Create
        public ActionResult Create()
        {
            var vm = new Application.ViewModels.NoteDetailViewModel();
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            vm.Notes = notes.Notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiers(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            vm.Tags = tags.Tags;
            return View(vm);
        }


        // POST: NoteController/POSTCreate/6
        [HttpPost]
        public ActionResult POSTCreate(NoteDetailViewModel noteDetailViewModel, int? page)
        {
            noteDetailViewModel.Note.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Application.Services.NotesControllerService.CreateNote(noteDetailViewModel.Note, noteDetailViewModel.newDossierName, noteDetailViewModel.Note.UserId, noteDetailViewModel.CheckedTags, noteDetailViewModel.newTagName) ;
            Application.ViewModels.NoteListViewModel noteListViewModel = new();
            var notes = Application.Services.NotesControllerService.GetNoteListOfUser(noteDetailViewModel.Note.UserId).Notes;
            noteListViewModel.Notes = notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiers(this.User).ListDossiers;
            noteListViewModel.ListDossiers = dossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
            noteListViewModel.Tags = tags;
            noteListViewModel.PaginatedNotes = new(notes, page ?? 0, 6);
            noteListViewModel.CustomTitle = "A new note was just added.";
            return View("Index", noteListViewModel);
        }
        
      
        // GET: NoteController/Edit/6
        public ActionResult Edit(int id)
        {
            var vm = new Application.ViewModels.NoteDetailViewModel();
            var note = Application.Services.NotesControllerService.GetNoteDetails(id).Note;
            vm.Note = note;
            vm.Note.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tagsOfNote = Application.Services.TagsControllerService.GetTagsOfNote(id);
            vm.TagsOfNote = tagsOfNote.TagsOfNote;
            var allTagsWithoutCurrents = Application.Services.TagsControllerService.GetTagsWithoutCurrents(tagsOfNote.TagsOfNote, vm.Note.UserId);
            vm.Tags = allTagsWithoutCurrents.Tags;
            vm.dossier = Application.Services.DossierControllerService.GetDossierName(vm.Note.Iddossier).newDossierName;
            var dossiers = Application.Services.DossierControllerService.GetListDossiers(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            return View(vm);
        }

        // POST: NoteController/POSTEdit/3
        [HttpPost]
        public ActionResult POSTEdit(NoteDetailViewModel noteDetailViewModel, int? page, int dossier)
        {
            noteDetailViewModel.Note.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Application.Services.NotesControllerService.EditNote(noteDetailViewModel.Note);
            Application.Services.NotesControllerService.AddTagsToNote(noteDetailViewModel.CheckedTags, noteDetailViewModel.Note.Idnote);
            Application.ViewModels.NoteListViewModel vm = new();
            vm = Application.Services.NotesControllerService.GetNoteListOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToAction(nameof(Index));
           
        }

        // GET: NoteController/DeleteNoteTag/3/2
        public ActionResult DeleteNoteTag(int id1, int id2)
        {
            Application.Services.TagsControllerService.DeleteNoteTag(id1, id2);
            Application.ViewModels.NoteDetailViewModel vm = new();
            vm.Note = Application.Services.NotesControllerService.GetNoteDetails(id2).Note;
            vm.Note.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tagsOfNote = Application.Services.TagsControllerService.GetTagsOfNote(id2);
            vm.TagsOfNote = tagsOfNote.TagsOfNote;
            var allTagsWithoutCurrents = Application.Services.TagsControllerService.GetTagsWithoutCurrents(tagsOfNote.TagsOfNote, vm.Note.UserId);
            vm.Tags = allTagsWithoutCurrents.Tags;
            vm.dossier = Application.Services.DossierControllerService.GetDossierName(vm.Note.Iddossier).newDossierName;
            var dossiers = Application.Services.DossierControllerService.GetListDossiers(this.User).ListDossiers;
            vm.ListDossiers = dossiers;
            return View("Edit", vm);;

        }

       // GET: NoteController/Delete/4
        public ActionResult Delete(int id)
        {
            Application.Services.NotesControllerService.DeleteNote(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: NoteController/TagView/3/1
        public ActionResult TagView(int id, int? page)
        {
            Application.ViewModels.NoteListViewModel vm = new();
            var notes = Application.Services.NotesControllerService.GetNoteListByTagId(this.User.FindFirstValue(ClaimTypes.NameIdentifier), id).Notes;
            vm.Notes = notes;
            var dossiers = Application.Services.DossierControllerService.GetListDossiersDashboard(this.User).ListDossiers;
            var tags = Application.Services.TagsControllerService.GetTagsOfUser(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Tags;
            vm.ListDossiers = dossiers;
            vm.Tags = tags;
            vm.PaginatedNotes = new(notes, page ?? 0, 6);
            var tagName = Application.Services.TagsControllerService.GetTagDetails(id).Tag.Nom;
            vm.CustomTitle = "Notes with #" + tagName;
            return View("Index", vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

