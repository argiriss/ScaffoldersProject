using Microsoft.AspNetCore.Identity;
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
        public string UserCardId { get; set; }

        //One to many relationship with cartitem
        public ICollection<CartItem> Items { get; set; }

    }
}
