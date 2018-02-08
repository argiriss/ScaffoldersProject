using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ScaffoldersProject.Models
{
    public class CartOrder
    {
        [Key]
        public int CartOrderId { get; set; }    

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public decimal Quantity { get; set; }
    }
}
