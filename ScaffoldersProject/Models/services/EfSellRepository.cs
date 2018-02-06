using Microsoft.AspNetCore.Identity;
using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfSellRepository:ISellRepository
    {
        private MainDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IQueryable<Sell> Sells => db.Sell;


        //Depedency injection implemented on constructor.Instatiate db object
        public EfSellRepository(MainDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        //Save to Order Table
        public async Task SellSave(Sell sell)
        {
            db.Sell.Add(sell);
            await db.SaveChangesAsync();
        }

        //Save to CartSell Table
        public async Task CartSellSave(CartSell cartSell)
        {
            db.CartSell.Add(cartSell);
            await db.SaveChangesAsync();
        }

        public async Task InstantSell(Sell instantSell, int productId, decimal coinSell)
        {
            //Add the sell to Sell Table and save it
            await SellSave(instantSell);

            //Calculate realtime from bid(offer) table
            var moneyEarned = await RealTimePrice(productId, coinSell);

            //Find current product price
            var product = await db.Products.FindAsync(productId);

            //Add to cartSell table the new sell
            CartSell newCartSell = new CartSell
            {
                SellId = instantSell.SellId,
                ProductId = productId,
                Quantity = coinSell
            };

            await CartSellSave(newCartSell);

            //Reduse the quantity of this product from portfolio
            var productNewQuantity = db.PortFolio.FirstOrDefault(x => x.ProductId == productId && x.UserPortofolioId==instantSell.UserSellId);
            productNewQuantity.CoinsQuantity -= newCartSell.Quantity;
            db.PortFolio.Update(productNewQuantity); //update the table
            await db.SaveChangesAsync();

            //Increase the Wallet amount by moneyEarn in parameters
            //Find Client Wallet and reduse it
            var clientUser = await _userManager.FindByIdAsync(instantSell.UserSellId);
            clientUser.Wallet += moneyEarned;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);
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

        public async Task UpdateTradeHistory(TradeHistory trade)
        {
            db.TradeHistory.Add(trade);
            await db.SaveChangesAsync();
        }

        public async Task SetCurrentPrice(int productId, decimal closedPrice)
        {
            var productFind = await db.Products.FindAsync(productId);
            productFind.Price = closedPrice;
            db.Products.Update(productFind);
            await db.SaveChangesAsync();
        }

        public async Task<decimal> RealTimePrice(int productId, decimal coinSell)
        {
            //List from the Offer  table sorted Descending
            var offersToBuy = db.Offer.Where(x => x.ProductId == productId).OrderByDescending(x => x.PriceOffer).ToList();
            decimal moneyEarn = 0; //money earned from selling the coinsell amount
            foreach (var item in offersToBuy)
            {
                if (item.Quantity >coinSell)
                {
                    //if offer in offer table is bigger than  the amount we enter
                    moneyEarn += coinSell * item.PriceOffer;
                    //reduce the quantity of the bid in offer table
                    item.Quantity -= coinSell;
                    db.Offer.Update(item);
                    await db.SaveChangesAsync();

                    decimal closedPrice = item.PriceOffer; //the new current product price
                    //Find current product price
                    await SetCurrentPrice(productId, closedPrice);

                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = coinSell,
                        Price = item.PriceOffer,
                        Status = "Sell",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                    break;
                }
                else if (item.Quantity ==coinSell)
                {
                    
                    db.Offer.Remove(item);

                    moneyEarn += coinSell * item.PriceOffer;//

                    decimal closedPrice = item.PriceOffer; //the new current product price

                    //Find current product price
                    await SetCurrentPrice(productId, closedPrice);

                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = item.Quantity,
                        Price = item.PriceOffer,
                        Status = "Sell",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                    break;
                }
                else
                {
                    db.Offer.Remove(item);
                    await db.SaveChangesAsync();

                    coinSell -= item.Quantity;

                    moneyEarn += item.Quantity * item.PriceOffer; //increase the quantity of user's order

                    TradeHistory trade = new TradeHistory //initiate a new trade
                    {
                        Quantity = item.Quantity,
                        Price = item.PriceOffer,
                        Status = "Sell",
                        DateofTransaction = DateTime.Now,
                        ProductId = item.ProductId
                    };
                    await UpdateTradeHistory(trade);
                    
                }
            }
            return moneyEarn;
        }
    }
}
