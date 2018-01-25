using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models;
using ScaffoldersProject.Models.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartSummaryViewComponent(ICartRepository cartRepository , UserManager<ApplicationUser> userManager)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            decimal totalCost = 0;
            Cart cartSummary = _cartRepository.Cart.FirstOrDefault(x => x.UserCartId == _userManager.GetUserId(HttpContext.User));
            if (cartSummary != null)
            {
                totalCost = _cartRepository.ComputeTotalCost(_userManager.GetUserId(HttpContext.User));
                ViewBag.total = totalCost;
                return View(cartSummary);
            }
            else
            {
                ViewBag.total = totalCost;
                return View(cartSummary);
            }
          
        }
    }
}
