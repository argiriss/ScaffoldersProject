using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.ManageViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
    }
}
