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
        private IWalletRepository _walletRepository;
        //Dioctionary with Key=userId and Value=connectionId
        private ConcurrentDictionary<string, string> OnlineUser { get; set; }
        
        //Depedency injection
        public MainHub(UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, 
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            IWalletRepository walletRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
        }

        ////When we invoke from client with Send value end send back message from parameter
        //public async Task Send(string message)
        //{
        //    await Clients.All.InvokeAsync("Send",message);
        //}

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

        public async Task Buy(string text)
        {
            Order orderObject = new Order { };
            //JObject Order = JObject.Parse(orderObject);
            //set order userId to order table
            orderObject.UserOrderId = _userManager.GetUserId(Context.User);
            orderObject.OrderDay = DateTime.Now;
            //Add order to database and save it with this method from EfOrderRepository passing
            //the order Object with the above properties
            await _orderRepository.AddNewOrder(orderObject);

            //var orderPrice = _cartRepository.GetOrderCost(orderObject.OrderID, findClientCart.CartId);
            //var orderPrice = 0;

            await Clients.Client(Context.ConnectionId).InvokeAsync("BuyItem","ok");
        }

        public async Task Deposit(string amount)
        {
            var desAmount = Convert.ToDecimal(amount);
            //Make the deposit
            await _walletRepository.Deposit(desAmount, _userManager.GetUserId(Context.User));
            //Calculate total amount in my wallet
            var totalAmount=await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            //Send to client balance area in left menu, the total amount 
            await Clients.Client(Context.ConnectionId).InvokeAsync("Success", totalAmount.ToString("C"));
        }

        public async Task Balance()
        {
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            await Clients.Client(Context.ConnectionId).InvokeAsync("Balance", totalAmount.ToString("C"));
        }

    }
}
