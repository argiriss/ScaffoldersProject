using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public ICollection<CartItem> Items { get; set; }
        [Required(ErrorMessage = "You have to provide a name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You have to provide an address!")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "You have to provide a city!")]
        public string City { get; set; }
        [Required(ErrorMessage = "You have to provide a country!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "You have to provide a zip code!")]
        public int Zip { get; set; }

        [BindNever]
        public bool Shipped { get; set; }
        public string UserOrderId { get; set; } //similar to foreign Key
    }
}
