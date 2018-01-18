﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class ImageViewModel
    {
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a product description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
        [Required]
        public int Stock { get; set; }

        public IFormFile Image { get; set; } 
      
    }
}