using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Cart
    {

        [Key]
        public int CartId { get; set; }
      
        public int ProductId { get; set; }
        public Products Product { get; set; }

        public decimal Quantity { get; set; }

        [BindNever]
        public string UserCartId { get; set; } //similar to foreign Key

    }
}
