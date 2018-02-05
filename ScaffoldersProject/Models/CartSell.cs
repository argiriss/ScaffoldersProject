using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class CartSell
    {
        [Key]
        public int CartSellId { get; set; }

        public int SellId { get; set; }
        public Sell Sell { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public decimal Quantity { get; set; }
    }
}
