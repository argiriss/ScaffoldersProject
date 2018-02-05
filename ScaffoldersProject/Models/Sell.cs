using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;


namespace ScaffoldersProject.Models
{
    public class Sell
    {
        //Client Sell
        [Key]
        public int SellId { get; set; }

        [BindNever]
        public DateTime SellDay { get; set; }

        [BindNever]
        public string UserSellId { get; set; } //similar to foreign Key
    }
}
