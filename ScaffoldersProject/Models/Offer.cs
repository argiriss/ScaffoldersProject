using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    //this class initialize objects that represent wannabe sell items
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceOffer { get; set; }
        public DateTime DateofOffer { get; set; } //date that seller enters his offer        
        public string UserOfferId { get; set; } //the user's id who makes the offer (like foreign key)
        public bool IsMatched { get; set; } //=false as far as the offer mismatch with the available asks

        public int ProductId { get; set; }
        public Products Product { get; set; }


    }
}
