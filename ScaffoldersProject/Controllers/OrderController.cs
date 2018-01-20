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
        public ViewResult OrderForm()
        {
            
            return View(new Order());
        }

        public IActionResult CompleteOrder(string Name, string Address1, string Address2, string City, string Country, int Zip)
        {
            Order order = new Order
            {
                Name = Name,
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                Country = Country,
                Zip = Zip,
                UserOrderId = _userManager.GetUserId(User)

            };
            //Find current User's cart and clear it
            Cart findCart = _cartRepository.Cart.First(x => x.UserCardId == _userManager.GetUserId(User));
          
            //then Add the Order object to Order table for the current User(foreign _key)
            //We have to pass the parameter cart findCart in method in order to find which
            //entries of CartItem must be marked as ordered
            _orderRepository.AddNewOrder(order, findCart);

            //Update the Product table by reducing the stock of ordered product respectively
            

            return View();
        }
    }
}