using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class SameCategoryViewModel
    {
        public Products Product { get; set; }
        public List<Products> SimilarProducts { get; set; }
        public ViewImageViewModel ViewImage { get; set; }

        public SameCategoryViewModel(Products p , List<Products> similarProducts, ViewImageViewModel viewImage)
        {
            Product = p;
            SimilarProducts = similarProducts;
            ViewImage = viewImage;
        }
    }
}
