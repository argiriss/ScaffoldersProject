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
        private CartItem cart;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IProductRepository repository, UserManager<ApplicationUser> userManager , CartItem theCart)
        {
            _repository = repository;
            _userManager = userManager;
            cart = theCart;



        }
        public IActionResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            //query for take cartitems for user id=current user .to list
            return View(cart);
        }

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
                //ADD THE PRODUCT TO REPOSITORY
                //THEN QUERY THE BASE TO TAKE ALL CARTITEMS WITH USERID=CURRENT USERID TO A LIST
                //RETURN VIEW CART WITH THIS LIST
            }
            return RedirectToRoute("cart");


        }




    }
}