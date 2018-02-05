using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class TradeHistory
    {
        [Key]
        public int TradeHistoryId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } //kind of transaction (buy or sell)
        public DateTime DateofTransaction { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}
