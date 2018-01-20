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
        private ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        //Depedency injection to constructor
        public CartController(
            IProductRepository repository,
            UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository)
        {
            _repository = repository;
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            //ViewBag.ReturnUrl = TempData["returnUrl"];
            ////Cart c = cart.Cart.SingleOrDefault(x => x.UserCardId == _userManager.GetUserId(User));

            //if (c == null)
            //{
            //    c = new Cart
            //    {
            //        UserCardId = _userManager.GetUserId(User)
            //    };
            //}
            ////ViewBag.Total = cart.ComputeTotalCost(c.CartId);
            return View();
        }

        public ActionResult AddToCart(int productId, int quantity, string returnUrl)
        {
            //Find the product with the given Id
            Products selectedProduct = _repository.Products.First(x => x.ProductId == productId);
            //Check Cart database if exist one with this userId and creates one if null
            var checkCartExist = _cartRepository.Cart.FirstOrDefault(x => x.UserCardId == _userManager.GetUserId(User));
            if (checkCartExist == null)
            {
                Cart cartNew = new Cart
                {
                    UserCardId = _userManager.GetUserId(User)
                };

                //Save cart to database for this user once and for all
                _cartRepository.CartSave(cartNew);
            }
            _cartRepository.AddItem(selectedProduct, quantity, checkCartExist);

           

            TempData["returnUrl"] = returnUrl;
            //return RedirectToAction("Index", new { c=c });
            
            var res = _cartRepository.Cart.First(x => x.UserCardId == _userManager.GetUserId(User));
            var total=_cartRepository.ComputeTotalCost(res);
            ViewBag.Total = total;
            return View("Index",res);
            //return RedirectToRoute("cart");
        }
    }
}