using Microsoft.AspNetCore.Identity;
using ScaffoldersProject.Data;
using ScaffoldersProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ScaffoldersProject.Models.services
{
    public class EfOrderRepository : IOrderRepository
    {
        private MainDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IQueryable<Order> Orders => db.Order;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfOrderRepository(MainDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        //Save to Order Table
        public async Task OrderSave(Order order)
        {
            db.Order.Add(order);
            await db.SaveChangesAsync();
        }
        //Save to CartOrder Table
        public async Task CartOrderSave(CartOrder cartOrder)
        {
            db.CartOrder.Add(cartOrder);
            await db.SaveChangesAsync();
        }
        //Reduse product stock quantity
        public async Task ProductReduseStock(Products product)
        {
            db.Products.Update(product); //update the table
            await db.SaveChangesAsync();
        }
        //method for Adding the new order to Order table
        public async Task AddNewOrder(Order orderDetails)
        {
            //Add the order to Order Table and save it
            await OrderSave(orderDetails);
            //Find the cart items List which exist in User's Cart
            var CartItemsOrdered = db.Cart.Where(x => x.UserCartId == orderDetails.UserOrderId).ToList();
            //for those Cart Items 
            CartOrder cartOrderTable = new CartOrder();
            foreach (var item in CartItemsOrdered)
            {
                //find Product price
                var findProduct = await db.Products.FindAsync(item.ProductId);
                //!!!!!!Inside foreach no Iquerable thats why i made CartItemsOrdered 
                //To List!!!!!!!!.........Important...............
                //Creation of cartOrder ID in CartOrder Table for transfer items from cart 
                //to cartOrder so as to clear cart table
                cartOrderTable.OrderId = orderDetails.OrderID;
                cartOrderTable.ProductId = item.ProductId;
                cartOrderTable.Quantity = item.Quantity / findProduct.Price;
                await CartOrderSave(cartOrderTable);
                //Remove item from cart table so as to clear this table after order placed
                db.Cart.Remove(item);
                //for each item removing,  reduce its stock by the buying quantity
                Products productReduseStock = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                productReduseStock.Stock -= item.Quantity / findProduct.Price; //reduce the stock
                await ProductReduseStock(productReduseStock);
                //if product not exist in portfolio insert it
                var check = db.PortFolio.FirstOrDefault(x => x.ProductId == item.ProductId && x.UserPortofolioId == orderDetails.UserOrderId);
                if (check != null)
                {
                    //if exists the raise its coin quantity
                    check.CoinsQuantity += item.Quantity / findProduct.Price;
                    db.PortFolio.Update(check); //update the table
                    await db.SaveChangesAsync();
                }
                else
                {
                    //insert new product to portfolio
                    Portfolio newproduct = new Portfolio
                    {
                        ProductId = item.ProductId,
                        CoinsQuantity = item.Quantity / findProduct.Price,
                        UserPortofolioId = orderDetails.UserOrderId
                    };
                    db.PortFolio.Add(newproduct);
                    await db.SaveChangesAsync();
                }
                //Reduse the Wallet amount by quantity which is the euro spend ,in parameters
                //Find Client Wallet and reduse it
                var clientUser = await _userManager.FindByIdAsync(orderDetails.UserOrderId);
                clientUser.Wallet -= item.Quantity;
                //Save the changes
                await _userManager.UpdateAsync(clientUser);
            }//End of foreach loop in cart table
        }
        public async Task InstantOrder(Order instantOrder, int productId, decimal euroSpend)
        {
            //Add the order to Order Table and save it
            await OrderSave(instantOrder);
            //Calculate realtime from ask table
            var quantityToBeOrdered = await RealTimePrice(productId, euroSpend);

            //Find current product price
            var product = await db.Products.FindAsync(productId);

            //Add to cartOrder table the new order
            CartOrder newCartOrder = new CartOrder
            {
                OrderId = instantOrder.OrderID,
                ProductId = productId,
                Quantity = quantityToBeOrdered
            };
            await CartOrderSave(newCartOrder);

            //if product not exist in portfolio insert it
            var check = db.PortFolio.FirstOrDefault(x => x.ProductId == productId && x.UserPortofolioId == instantOrder.UserOrderId);
            if (check != null)
            {
                //if exists the raise its coin quantity
                check.CoinsQuantity += newCartOrder.Quantity;
                db.PortFolio.Update(check); //update the table
                await db.SaveChangesAsync();
            }
            else
            {
                //insert new product to portfolio
                Portfolio newproduct = new Portfolio
                {
                    ProductId = productId,
                    CoinsQuantity = newCartOrder.Quantity,
                    UserPortofolioId = instantOrder.UserOrderId
                };
                db.PortFolio.Add(newproduct);
                await db.SaveChangesAsync();
            }
            //Reduse the Wallet amount by euroSpend in parameters
            //Find Client Wallet and reduse it
            var clientUser = await _userManager.FindByIdAsync(instantOrder.UserOrderId);
            clientUser.Wallet -= euroSpend;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);
        }
        public async Task<List<Products>> GetAllApprovedProducts()
        {
            var getAllProducts = db.Products.Where(x => x.AdminApproved == true).ToList();
            return getAllProducts;
            //var clientOrders = db.Order.Where(x => x.UserOrderId == userId).ToList();
            //var clientOrdersIds = clientOrders.Select(x => x.OrderID).ToList();
            //var orders = db.CartOrder.Where(x => clientOrdersIds.Contains(x.OrderId)).ToList();
            //var productsIds = orders.Select(x => x.ProductId).Distinct().ToList();
            //var orderedProducts = db.Products.Where(x => productsIds.Contains(x.ProductId)).ToList();
            //return orderedProducts;
        }
        public decimal ClientSpecificProductTotal(int productId, string userId)
        {
            var product = db.PortFolio.FirstOrDefault(x => x.ProductId == productId && x.UserPortofolioId == userId);
            if (product != null)
            {
                return product.CoinsQuantity;
            }
            else
            {
                return 0;
            }
        }
        public async Task<decimal> GetProductCuurentPrice(int productId)
        {
            var productFind = await db.Products.FindAsync(productId);
            return productFind.Price;
        }
        public async Task UpdateTradeHistory(TradeHistory trade)
        {
            db.TradeHistory.Add(trade);
            await db.SaveChangesAsync();
        }
        public List<TradeHistory> GetTradeHistory(int productId)
        {
            return db.TradeHistory.Where(x => x.ProductId == productId).OrderByDescending(x => x.DateofTransaction).ToList();
        }
        public async Task SetCurrentPrice(int productId, decimal closedPrice)
        {
            var productFind = await db.Products.FindAsync(productId);
            productFind.Price = closedPrice;
            db.Products.Update(productFind);
            await db.SaveChangesAsync();
        }
        public async Task<decimal> RealTimePrice(int productId, decimal euroSpend)
        {
            //List from the ask table sorted ascending
            var offersToBuy = db.Ask.Where(x => x.ProductId == productId).OrderBy(x => x.PriceAsk).ToList();
            decimal quantityToBeOrdered = 0; //the quantity that users can buy with his euroSpend
            foreach (var item in offersToBuy)
            {
                if ((item.PriceAsk * item.Quantity) % euroSpend == item.PriceAsk * item.Quantity)
                {
                    euroSpend -= item.PriceAsk * item.Quantity;
                    db.Ask.Remove(item);
                    quantityToBeOrdered += item.Quantity; //increase the quantity of user's order
                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = item.Quantity,
                        Price = item.PriceAsk,
                        Status = "Buy",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                }
                else if (item.PriceAsk * item.Quantity == euroSpend)
                {
                    euroSpend -= item.PriceAsk * item.Quantity;
                    db.Ask.Remove(item);
                    quantityToBeOrdered += item.Quantity; //increase the quantity of user's order
                    decimal closedPrice = item.PriceAsk; //the new current product price
                    //Find current product price
                    await SetCurrentPrice(productId, closedPrice);
                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = item.Quantity,
                        Price = item.PriceAsk,
                        Status = "Buy",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                    break;
                }
                else
                {
                    //reduce the quanity of ask
                    var itemForReduce = item;
                    itemForReduce.Quantity -= euroSpend / item.PriceAsk;
                    db.Ask.Update(itemForReduce);
                    await db.SaveChangesAsync();
                    quantityToBeOrdered += euroSpend / item.PriceAsk; //increase the quantity of user's order
                    decimal closedPrice = item.PriceAsk; //the new current product price
                    await SetCurrentPrice(productId, closedPrice);
                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = euroSpend / item.PriceAsk,
                        Price = item.PriceAsk,
                        Status = "Buy",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                    break;
                }
            }
            return quantityToBeOrdered;
        }
    }
}
