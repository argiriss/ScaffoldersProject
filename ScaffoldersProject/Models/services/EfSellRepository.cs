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

            //Find current product price
            var product = await db.Products.FindAsync(productId);

            //Add to cartSell table the new order
            CartSell newCartSell = new CartSell
            {
                SellId = instantSell.SellId,
                ProductId = productId,
                Quantity = coinSell
            };

            await CartSellSave(newCartSell);

            //Reduse the quantity of this product from portfolio
            var productNewQuantity = db.PortFolio.FirstOrDefault(x => x.ProductId == productId);
            productNewQuantity.CoinsQuantity -= newCartSell.Quantity;

            //Reduse the Wallet amount by euroSpend in parameters
            //Find Client Wallet and reduse it
            var clientUser = await _userManager.FindByIdAsync(instantSell.UserSellId);
            clientUser.Wallet += coinSell*product.Price;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);
        }

        public async Task<List<Products>> GetClientSells(string userId)
        {
            var clientSells = db.Sell.Where(x => x.UserSellId == userId).ToList();
            var clientSellIds = clientSells.Select(x => x.SellId).ToList();
            var sells = db.CartSell.Where(x => clientSellIds.Contains(x.SellId)).ToList();
            var productsIds = sells.Select(x => x.ProductId).Distinct().ToList();
            var soldProducts = db.Products.Where(x => productsIds.Contains(x.ProductId)).ToList();
            return soldProducts;
        }

        public decimal ClientSpecificProductTotal(int productId, string userId)
        {
            //find all user orders in order table
            var clientSell = db.Sell.Where(x => x.UserSellId == userId).ToList();
            var clientSellIds = clientSell.Select(x => x.SellId).ToList();
            var sells = db.CartSell.Where(x => clientSellIds.Contains(x.SellId) && x.ProductId == productId);
            var sum = sells.Select(x => x.Quantity).Sum();
            return sum;
        }
    }
}
