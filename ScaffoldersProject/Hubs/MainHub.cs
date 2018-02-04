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
        private IAskRepository _askRepository;
        private IOfferRepository _offerRepository;
        
        //Dioctionary with Key=userId and Value=connectionId
        private ConcurrentDictionary<string, string> OnlineUser { get; set; }
        
        //Depedency injection
        public MainHub(UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, 
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            IWalletRepository walletRepository,
            IAskRepository askRepository,
            IOfferRepository offerRepository
           )
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
            _askRepository = askRepository;
            _offerRepository = offerRepository;
        }

        //This happens on connection
        public async override Task OnConnectedAsync()
        {
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            var clientOrders = await _orderRepository.GetClientOrders(_userManager.GetUserId(Context.User));
            await Clients.Client(Context.ConnectionId).InvokeAsync("Wallet", totalAmount.ToString("C"),clientOrders);
            //place a list of bids for a specific product in Order Book
            //var asksTable = _askRepository.Asks.Where(x=>x.ProductId==5).ToList();  
            //await Clients.All.InvokeAsync("PlaceBid", asksTable);
        }

        public async Task Deposit(string amount)
        {
            var desAmount = Convert.ToDecimal(amount);
            //Make the deposit
            await _walletRepository.Deposit(desAmount, _userManager.GetUserId(Context.User));
            //Calculate total amount in my wallet
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            //Send to client balance area in left menu, the total amount 
            await Clients.Client(Context.ConnectionId).InvokeAsync("Success", totalAmount.ToString("C"));

            var depositHistoryTable = _walletRepository.GetDepositHistory();
            await Clients.All.InvokeAsync("NewOrder", depositHistoryTable);
        }

        public async Task Wallet()
        {
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            await Clients.Client(Context.ConnectionId).InvokeAsync("Wallet", totalAmount.ToString("C"));
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

        //Clear cart from all products items
        public async Task ClearCart()
        {
            await _cartRepository.Clear(_userManager.GetUserId(Context.User));
            var totalCost = 0;
            await Clients.Client(Context.ConnectionId).InvokeAsync("Clear",totalCost.ToString("c"));
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
            var totalCost = 0;

            await Clients.Client(Context.ConnectionId).InvokeAsync("BuyItem","ok",totalCost.ToString("c"));
            //await Clients.All.InvokeAsync("NewOrder", table);
        }

        //Instant buy from sidenav menu
        public async Task InstantBuy(int productId,decimal euroSpend,decimal quantity)
        {
            Order instantOrder = new Order { };
            //JObject Order = JObject.Parse(orderObject);
            //set order userId to order table
            instantOrder.UserOrderId = _userManager.GetUserId(Context.User);
            instantOrder.OrderDay = DateTime.Now;
            //Add order to database and save it with this method from EfOrderRepository passing
            //the order Object with the above properties
            await _orderRepository.InstantOrder(instantOrder, productId,euroSpend,quantity);
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            var totalFromThisProduct = _orderRepository.ClientSpecificProductTotal(productId, instantOrder.UserOrderId);
            await Clients.Client(Context.ConnectionId).InvokeAsync("InstantBuy", totalAmount.ToString("C"), totalFromThisProduct.ToString("C"));
        }
       
        //when user place a bid and asks a price and a quanity for specific product
        public async Task PlaceBid(int productId , decimal desiredPrice, double desiredQuantity)
        {   
            //Add new bid to Ask table
            await _askRepository.AddAsk(_userManager.GetUserId(Context.User), desiredPrice, desiredQuantity, productId);
            //take the list of bids from Ask table
            var asksTable = _askRepository.Asks.Where(x=>x.ProductId== productId);
            await Clients.All.InvokeAsync("PlaceBid", asksTable);    
        }

        //Pass ask offer lists on orderbook
        public async Task SelectedCoin(int productId)
        {
            var askList = _askRepository.Asks.Where(x => x.ProductId == productId).ToList();
            var offerList = _offerRepository.Offers.Where(x => x.ProductId == productId).ToList();
            var totalFromThisProduct = _orderRepository.ClientSpecificProductTotal(productId, _userManager.GetUserId(Context.User));
            var currentProductPrice = await _productRepository.GetCurrentPrice(productId);
            var totalInEuro = totalFromThisProduct * currentProductPrice;
            await Clients.All.InvokeAsync("SelectedCoin", totalFromThisProduct, totalInEuro);

            _orderRepository.ClientSpecificProductTotal(productId, _userManager.GetUserId(Context.User));
        }
    }
}
