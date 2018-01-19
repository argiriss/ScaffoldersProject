using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Identity;

namespace ScaffoldersProject.Controllers
{
    public class OrderController : Controller
    {
        private IProductRepository _repository;
        private ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private IOrderRepository _orderRepository;

        public OrderController(IProductRepository repository , ICartRepository cartRepository, UserManager<ApplicationUser> userManager,  IOrderRepository orderRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        //returns the form to submit order details
        public IActionResult OrderForm()
        {
            return View();
        }
    }
}