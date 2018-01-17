using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using Microsoft.AspNetCore.Identity;
using ScaffoldersProject.Models;

namespace ScaffoldersProject.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;
        private Cart cart;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IProductRepository repository, UserManager<ApplicationUser> userManager ,Cart cartService)
        {
            _repository = repository;
            _userManager = userManager;
            cart = cartService;
        }

        public IActionResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            //QUERY STON PINAKA Cart .where(x=>x.UserId==_userManager.GetUserId(HttpContext.User)) 
            //GIA NA EMFANISEI TO CART TOU CURRENT USER
            return View(cart);
        }

        public ActionResult AddToCart(int ProductId, int Quantity, string returnUrl)
        {
            Products product = _repository.Products.SingleOrDefault(x => x.ProductId == ProductId);
            string cid = _userManager.GetUserId(HttpContext.User);
            
            if (product != null)
            {
                cart.AddProduct(product, Quantity); //PROSTHETOUME TO PROION KAI TIN POSOTITA POU DIALEKSE O XRISTIS STHN METAVLITI CART
                cart.UserId = _userManager.GetUserId(HttpContext.User);//EKXWROUME TO CURRENT USERID
                cart.Checkout = false;
                //UPDATE STON PINAKA CART .where(x=>x.UserId==_userManager.GetUserId(HttpContext.User)
            }
            TempData["returnUrl"] = returnUrl;
            return RedirectToAction("Index", new { cart = cart });
            //return RedirectToRoute("cart");
        }
    }
}