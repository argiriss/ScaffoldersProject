using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class SaveProductsViewModel
    {

        public int ProductId { get; set; }

        public Products Products { get; set; }
        
        public bool AdminApproved { get; set; }
    }
}
