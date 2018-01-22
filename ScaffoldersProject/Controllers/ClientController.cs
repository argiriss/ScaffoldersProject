using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;


namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private IProductRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private Cart cart;

        //Constructor depedency injection 
        public ClientController(IProductRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;

        }
        //End of Dionisis


        public IActionResult Index()
        {
            //Turn repo.Products into a List of View Model to return in the View
            var viewImageList = new List<ViewImageViewModel>();
            foreach (var product in _repository.Products)
            {
                var viewImage = new ViewImageViewModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ContentType = product.ContentType
                };

                if (product.Image != null)
                {
                    var ms = new MemoryStream(product.Image);
                    byte[] imageBytes = ms.ToArray();
                    viewImage.Image = Convert.ToBase64String(imageBytes);
                }
                viewImageList.Add(viewImage);
            }

           HttpContext.Session.SetString("Data", "From session");//Do we need this?
            return View(viewImageList);
        }

        //Θα καλειται οταν ο client κλικαρει σε ενα προιον και θα του το εμφανιζει μονο του με τα details
        public IActionResult ViewProduct(int productId)
        {
            ViewBag.Data = HttpContext.Session.GetString("Data");
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            var viewImage = new ViewImageViewModel();
            if(product.Image != null)
            {
                var ms = new MemoryStream(product.Image);
                byte[] imageBytes = ms.ToArray();
                viewImage.Image = Convert.ToBase64String(imageBytes);
            }
                      
            List<Products> similarProducts = _repository.Products.Where(i => i.Category == product.Category).ToList();            
            SameCategoryViewModel sameCategory = new SameCategoryViewModel(product , similarProducts, viewImage);
            return View(sameCategory);
        }

        //ΚΑΛΕΙΤΑΙ ΟΤΑΝ Ο CLIENT ΠΑΤΑΕΙ TO KOYΜΠΙ ADDTOCART
        //public RedirectToRouteResult AddToCart(int ProductId, int q)
        //{
        //    Products product = _repository.Products.SingleOrDefault(x => x.ProductId == ProductId);
        //    string cid = _userManager.GetUserId(HttpContext.User);
        //    CartItem c = null;

        //    if (product != null)
        //    {
        //        c = new CartItem(product, q)
        //        {
        //            ClientId = cid
        //        };
        //    }
           
        //    return RedirectToRoute("cart");


        //}

    }
}