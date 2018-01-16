﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Identity;

namespace ScaffoldersProject.Controllers
{
    public class ClientController : Controller
    {
        private IProductRepository _repository;  
        private Cart cart;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(IProductRepository repository , UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
            
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

        //ΚΑΛΕΙΤΑΙ ΟΤΑΝ Ο CLIENT ΠΑΤΑΕΙ TO KOYΜΠΙ ADDTOCART
        public RedirectToRouteResult AddToCart(int ProductId, int q)
        {
            Products product = _repository.Products.SingleOrDefault(x => x.ProductId == ProductId);
            string cid = _userManager.GetUserId(HttpContext.User);
            CartItem c = null;

            if (product != null)
            {
                c = new CartItem(product, q)
                {
                    ClientId = cid
                };
            }
           
            return RedirectToRoute("cart");


        }

    }
}