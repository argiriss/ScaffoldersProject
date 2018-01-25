using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ScaffoldersProject.Models;
using ScaffoldersProject.Models.services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;

namespace ScaffoldersProject.Hubs
{
    public class MainHub:Hub
    {
        //To provide you a quick overview, the Hub is the center piece of the SignalR. 
        //Similar to the Controller in ASP.NET MVC, a Hub is responsible for receiving 
        //input and generating the output to the client.
        private readonly UserManager<ApplicationUser> _userManager;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        //Dioctionary with Key=userId and Value=connectionId
        private ConcurrentDictionary<string, string> OnlineUser { get; set; }
        
        //Depedency injection
        public MainHub(UserManager<ApplicationUser> userManager,ICartRepository cartRepository, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        //When we invoke from client with Send value end send back message from parameter
        public async Task Send(string message)
        {
            await Clients.All.InvokeAsync("Send",message);
        }

        //When we invoke from client with SendClient value
        public  async  Task RemoveItem(int productId)
        {
            var productName = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId).Name;
            ////First we look for a Cart with the login User Id
            var findClientCart = _cartRepository.Cart.FirstOrDefault(x => x.UserCartId == _userManager.GetUserId(Context.User));
            ////With the product id and the cart id we invoke the function remove item
            _cartRepository.RemoveItem(productId, findClientCart.UserCartId);
            ////After the removal we compute the new total cost
            var totalCost = _cartRepository.ComputeTotalCost(findClientCart.UserCartId);
            await Clients.Client(Context.ConnectionId).InvokeAsync("Remove", $"Product {productName} was successfully removed from your cart!", totalCost.ToString("C"));
        }

        public async Task Buy(Order orderObject)
        {
            //JObject Order = JObject.Parse(orderObject);
            //First we look for a Cart with the login User Id
            //var findClientCart = _cartRepository.Cart.FirstOrDefault(x => x.UserCardId == _userManager.GetUserId(Context.User));
            //orderObject.UserOrderId = _userManager.GetUserId(Context.User);
            
            //_orderRepository.AddNewOrder(orderObject, findClientCart);
            //var orderPrice =_cartRepository.GetOrderCost(orderObject.OrderID, findClientCart.CartId);


            //await Clients.All.InvokeAsync("Buy","Order saved in database", orderPrice);
        }

    }
}
