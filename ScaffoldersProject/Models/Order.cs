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
        //Client Buy
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDay { get; set; }

        [BindNever]
        public string UserOrderId { get; set; } //similar to foreign Key

    }
}
