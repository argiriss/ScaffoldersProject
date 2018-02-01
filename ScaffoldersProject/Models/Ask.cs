using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Ask
    {
        [Key]
        public int AskId { get; set; }
        public double Quantity { get; set; }
        public decimal PriceAsk { get; set; }
        public DateTime DateofAsk { get; set; } //date that seller enters his offer        
        public string UserAskId { get; set; } //the user's id who makes the offer (like foreign key)
        public bool IsMatched { get; set; } //=false as far as the offer mismatch with the available asks

        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}
