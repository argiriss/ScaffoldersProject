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
            Cart cartSummary = _cartRepository.Cart.FirstOrDefault(x => x.UserCardId == _userManager.GetUserId(HttpContext.User));
            var total = _cartRepository.ComputeTotalCost(cartSummary);
            ViewBag.Total = total;
            return View(cartSummary);
        }

    }
}
