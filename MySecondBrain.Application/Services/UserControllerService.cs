using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.Services
{
    public class UserControllerService
    {
        /// <summary>
        /// Appelle le service de la couche domain pour avoir la liste des users
        /// </summary>
        /// <returns></returns>
        public static ViewModels.UserListViewModel GetListUsers()
        {

            var users = Domain.Services.UserService.GetListUsers();

            ViewModels.UserListViewModel vm = new ViewModels.UserListViewModel()
            {
                ListUsers = users
            };

            return vm;
        }
        /// <summary>
        /// Appelle le service de la couche domain pour avoir le role d'un user en fonction de son id
        /// </summary>
        /// <param name="userId">user</param>
        /// <returns></returns>
        public static string GetRoleIdOfUser(string userId)
        {

            var roleId = Domain.Services.UserService.GetRoleIdOfUser(userId);

            

            return roleId;
        }

    }
}
