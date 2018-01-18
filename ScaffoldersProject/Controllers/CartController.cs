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
        private ICartRepository cart;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IProductRepository repository, UserManager<ApplicationUser> userManager ,ICartRepository cartService)
        {
            _repository = repository;
            _userManager = userManager;
            cart = cartService;
        }

        public IActionResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            Cart c = cart.Cart.SingleOrDefault(x => x.UserCardId == _userManager.GetUserId(User));

            if (c == null)
            {
                c = new Cart
                {
                    UserCardId = _userManager.GetUserId(User)
                };
            }
            ViewBag.Total = cart.ComputeTotalCost(c.CartId);
            return View(c);
        }

        public ActionResult AddToCart(int ProductId, int Quantity, string returnUrl)
        {
            Cart c = cart.Cart.SingleOrDefault(x => x.UserCardId == _userManager.GetUserId(User));

            if (c==null)
            {
                c = new Cart
                {
                    UserCardId = _userManager.GetUserId(User)
                }; 
            }

            Products product = _repository.Products.SingleOrDefault(x => x.ProductId == ProductId);
            string cid = _userManager.GetUserId(HttpContext.User);
            
            if (product != null)
            {
                
                cart.AddItem(product, Quantity , c.CartId); //PROSTHETOUME TO PROION KAI TIN POSOTITA POU DIALEKSE O XRISTIS STHN METAVLITI CART
                //cart.UserId = _userManager.GetUserId(HttpContext.User);//EKXWROUME TO CURRENT USERID
                //cart.Checkout = false;
                //UPDATE STON PINAKA CART .where(x=>x.UserId==_userManager.GetUserId(HttpContext.User)
            }
            TempData["returnUrl"] = returnUrl;
            return RedirectToAction("Index", new { c=c });
            //return RedirectToRoute("cart");
        }
    }
}