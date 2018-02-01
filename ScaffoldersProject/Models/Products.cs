﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Products
    {
        //DbSet in MainDbContext in Data folder
        [Key]
        public int ProductId { get; set; }        
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a product description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a Short Name")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Please enter exactly 3 characters only")]
        public string ShortName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
        public bool AdminApproved { get; set; }
        [Required(ErrorMessage = "Please specify the amount you wish to sell")]
        public int Stock { get; set; }
        //Image properties
        public byte[] Image { get; set; }
        public string ContentType { get; set; }

    }
}
