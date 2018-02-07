using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ScaffoldersProject.Data;
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
    public class MainHub : Hub
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
        private ISellRepository _sellRepository;

        //Dioctionary with Key=userId and Value=connectionId
        private ConcurrentDictionary<string, string> OnlineUser { get; set; }

        //Depedency injection
        public MainHub(UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IWalletRepository walletRepository,
            IAskRepository askRepository,
            IOfferRepository offerRepository,
            ISellRepository sellRepository
           )
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
            _askRepository = askRepository;
            _offerRepository = offerRepository;
            _sellRepository = sellRepository;
        }

        //This happens on connection
        public async override Task OnConnectedAsync()
        {
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            //list with all products for select products menu
            var getAllApprovedProducts = await _orderRepository.GetAllApprovedProducts();
            await Clients.Client(Context.ConnectionId).InvokeAsync("Wallet", totalAmount.ToString("C"), getAllApprovedProducts);
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
        public async Task RemoveItem(int productId)
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
            await Clients.Client(Context.ConnectionId).InvokeAsync("Clear", totalCost.ToString("c"));
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
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            await Clients.Client(Context.ConnectionId).InvokeAsync("BuyItem", "ok", totalCost.ToString("c"), totalAmount.ToString("c"));
        }

        //Instant buy from sidenav menu
        //We have to inform Wallet ,new Balance Euro and selected coin,remove from orderbook
        //bid view,and change product price if needed
        public async Task InstantBuy(int productId, decimal euroSpend)
        {
            await CheckWallet(euroSpend);
            Order instantOrder = new Order { };
            //JObject Order = JObject.Parse(orderObject);
            //set order userId to order table
            instantOrder.UserOrderId = _userManager.GetUserId(Context.User);
            instantOrder.OrderDay = DateTime.Now;

            //Add order to database and save it with this method from EfOrderRepository passing
            //the order Object with the above properties
            
            await _orderRepository.InstantOrder(instantOrder,productId,euroSpend);

            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            decimal totalFromThisProduct = _orderRepository.ClientSpecificProductTotal(productId, _userManager.GetUserId(Context.User));
            var currentProductPrice = await _productRepository.GetCurrentPrice(productId);
            //Our total coins in Euro 
            var totalInEuro = totalFromThisProduct * currentProductPrice;

            await Clients.Client(Context.ConnectionId).InvokeAsync("InstantBuySell", totalAmount.ToString("C"), totalFromThisProduct,totalInEuro);
            await SelectedCoin(productId);
        }

        public async Task InstantSell(int productId, decimal coinSell)
        {
            Sell newSell = new Sell
            {
                UserSellId = _userManager.GetUserId(Context.User),
                SellDay = DateTime.Now
            };
            //Save new sell to database
            await _sellRepository.InstantSell(newSell, productId, coinSell);
            //Calculate new wallet
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            //Query in portofolio to find new total from this product
            decimal totalFromThisProduct = _sellRepository.ClientSpecificProductTotal(productId, _userManager.GetUserId(Context.User));
            var currentProductPrice = await _productRepository.GetCurrentPrice(productId);
            //Our total coins in Euro 
            var totalInEuro = totalFromThisProduct * currentProductPrice;
            await Clients.Client(Context.ConnectionId).InvokeAsync("InstantBuySell", totalAmount.ToString("C"), totalFromThisProduct, totalInEuro);
            await SelectedCoin(productId);
        }

        //The seller sets his price at $30. That’s his ask price.

        //You are willing to pay $20 for the card.That your bid price

        //when user place a bid and asks a price and a quanity for specific product
        public async Task PlaceBid(int productId, decimal bidAmount, decimal limitPrice)
        {
            //Add new bid to offer table
            await _offerRepository.AddOffer(productId, bidAmount, limitPrice, _userManager.GetUserId(Context.User));
            //take the list of bids from Offer table
            var tempbidTable = _offerRepository.Offers.Where(x => x.ProductId == productId).ToList();
            var bidTable = tempbidTable.GroupBy(x => x.PriceOffer).Select(y => new { PriceOffer = y.First().PriceOffer, Quantity = y.Sum(s => s.Quantity) }).OrderByDescending(t => t.PriceOffer);
            await Clients.All.InvokeAsync("PlaceBid", bidTable);
        }

        public async Task PlaceAsk(int productId, decimal askAmount, decimal limitPrice)
        {
            //Add new Ask to ask table
            await _askRepository.AddAsk(productId, askAmount, limitPrice, _userManager.GetUserId(Context.User));
            //take the list of asks from ask table
            var tempAsk = _askRepository.Asks.Where(x => x.ProductId == productId).ToList();
            var askTable = tempAsk.GroupBy(x => x.PriceAsk).Select(y => new { PriceAsk = y.First().PriceAsk, Quantity = y.Sum(s => s.Quantity) }).OrderByDescending(t => t.PriceAsk);

            await Clients.All.InvokeAsync("PlaceAsk", askTable);
        }

        //Pass ask offer lists on orderbook
        public async Task SelectedCoin(int productId)
        {
            //.....................Notifications for all users..................................
            //List with all products for select products menu
            var getAllApprovedProducts = await _orderRepository.GetAllApprovedProducts();
            //Current product price
            var currentPrice = await _productRepository.GetCurrentPrice(productId);
            //take the list of bids from Offer table
            var tempbidTable = _offerRepository.Offers.Where(x => x.ProductId == productId).ToList();
            var bidTable=tempbidTable.GroupBy(x => x.PriceOffer).Select(y => new { PriceOffer = y.First().PriceOffer, Quantity = y.Sum(s => s.Quantity) }).OrderByDescending(t => t.PriceOffer);
            //take the list of asks from ask table
            var tempAsk = _askRepository.Asks.Where(x => x.ProductId == productId).ToList();
            var askTable = tempAsk.GroupBy(x=>x.PriceAsk).Select(y=>new { PriceAsk = y.First().PriceAsk, Quantity = y.Sum(s => s.Quantity) }).OrderByDescending(t=>t.PriceAsk);
            
            //Send tradeHistory to orderbook descending
            var tradeHistory = _orderRepository.GetTradeHistory(productId);   
            await Clients.All.InvokeAsync("SelectedCoinAll", getAllApprovedProducts, bidTable,askTable,currentPrice,tradeHistory);

            //..............Notify for specific client.................................
            //Quantity of selected product and total price in euro
            var totalFromThisProduct = _orderRepository.ClientSpecificProductTotal(productId, _userManager.GetUserId(Context.User));
            var totalInEuro = totalFromThisProduct * currentPrice;
            await Clients.Client(Context.ConnectionId).InvokeAsync("SelectedCoin", totalFromThisProduct, totalInEuro);
        }

        public async Task<bool> CheckWallet(decimal euros)
        {
            var totalAmount = await _walletRepository.TotalInMyWallet(_userManager.GetUserId(Context.User));
            if (euros > totalAmount)
            {
                await Clients.Client(Context.ConnectionId).InvokeAsync("Errors", "The amount is bigger from you wallet");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
