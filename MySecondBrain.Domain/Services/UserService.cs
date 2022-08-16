using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Domain.Services
{
    public class UserService
    {
        /// <summary>
        /// Renvoie tous les users de la db
        /// </summary>
        /// <returns>Liste des users</returns>
        public static List<Infrastructure.DB.AspNetUser> GetListUsers()
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.AspNetUsers.ToList();
            }
        }

        /// <summary>
        /// Renvoie tous les users de la db
        /// </summary>
        /// <returns>Liste des users</returns>
        public static string GetRoleIdOfUser(string userId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                var userRole = db.AspNetUserRoles.Where(x => x.UserId == userId).SingleOrDefault();
                return userRole.RoleId;
            }
        }
    }
}
