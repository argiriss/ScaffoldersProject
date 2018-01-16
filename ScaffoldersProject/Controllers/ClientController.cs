using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;

namespace ScaffoldersProject.Controllers
{
    public class ClientController : Controller
    {
        private IProductRepository _repository;
        private Cart cart;

        //Constructor depedency injection 
        public ClientController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Products);
        }

        //Θα καλειται οταν ο client κλικαρει σε ενα προιον και θα του το εμφανιζει μονο του με τα details
        public IActionResult ViewProduct(int productId)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            List<Products> SimilarProducts = _repository.Products.Where(i => i.Category == product.Category).ToList();
            SameCategoryViewModel r = new SameCategoryViewModel(product , SimilarProducts);
            return View(r);
        }

        public IActionResult AddToCart(Products prod , int q)
        {
            Products product = _repository.Products.SingleOrDefault(x => x == prod);
            if (product != null)
            {
                cart.AddProduct(product, q);
            }
            return View();
        }

    }
}