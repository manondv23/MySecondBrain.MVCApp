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
namespace MySecondBrain.MVCApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index() 
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Application.Services.UserControllerService.GetRoleIdOfUser(userId).Equals("1"))
            {
                Application.ViewModels.UserListViewModel vm = new();
                vm.ListUsers = Application.Services.UserControllerService.GetListUsers().ListUsers;
                return View(vm);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
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
