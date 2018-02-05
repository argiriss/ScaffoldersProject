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
            var cartList = _cartRepository.Cart.Where(x => x.UserCartId == _userManager.GetUserId(User));
            var total = _cartRepository.ComputeTotalCost(_userManager.GetUserId(User));
            ViewBag.Total = total;
            ViewBag.UserId = _userManager.GetUserId(User);
            return View(cartList);//return Res inside view
        }


        public async Task<ActionResult> AddToCart(int productId, decimal quantity, string returnUrl)
        {

            //Find a product with the given Id from the parameters
            Products product = _repository.Products.SingleOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                // if you add to cart from product view, add it with quantity 1 instead of 0
                if (quantity == 0)
                {
                    quantity = 1;
                }
                await _cartRepository.AddItem(product, quantity, _userManager.GetUserId(User));
            }
            
            return Redirect("Index");
        }
    }
}