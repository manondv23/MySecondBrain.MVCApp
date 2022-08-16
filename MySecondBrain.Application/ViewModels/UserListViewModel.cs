using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.ViewModels
{
    public class UserListViewModel
    {
        public List<Infrastructure.DB.AspNetUser> ListUsers { get; set; } 
    }
}
