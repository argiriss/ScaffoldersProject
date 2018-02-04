using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfCartRepository : ICartRepository
    {
        private MainDbContext db;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfCartRepository(MainDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Cart> Cart => db.Cart;

        public async Task CartSave(Cart cart)
        {
            db.Cart.Add(cart);
            await db.SaveChangesAsync();
        }

        public async Task AddItem(Products product, decimal quantity, string userId)
        {
            //Check If we find a cartId from this user
            Cart findProductInCart = db.Cart.FirstOrDefault(x => x.UserCartId == userId);
            if (findProductInCart != null)
            {
                //If item exists then we look for similar productId 
                Cart findInCart = db.Cart.FirstOrDefault(x => x.ProductId == product.ProductId && x.UserCartId == userId);
                if (findInCart != null)
                {
                    findInCart.Quantity += quantity;
                    db.Cart.Update(findInCart);
                    await db.SaveChangesAsync();
                }
                else
                {
                    //If there is not similar product then
                    findInCart = new Cart
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        UserCartId = userId
                    };
                    await CartSave(findInCart);
                }
            }
            else
            {
                //If we dont find a cart then we create a new one with userId
                findProductInCart = new Cart
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    UserCartId = userId
                };
                await CartSave(findProductInCart);
            }
        }

        //Clear all rpoducts from cart
        public async Task Clear(string userId)
        {
            var CartClear = db.Cart.Where(x => x.UserCartId == userId).ToList();
            foreach (var item in CartClear)
            {
                db.Cart.Remove(item);
            }
            await db.SaveChangesAsync();
        }


        public decimal ComputeTotalCost(string userId)
        {
            decimal totalCost = 0;
            //find list of items with same user id
            var findListOfItems = db.Cart.Where(x => x.UserCartId == userId);
            foreach (var item in findListOfItems)
            {
                decimal price = db.Products.First(x => x.ProductId == item.ProductId).Price;
                totalCost += item.Quantity;
            }
            return totalCost;
        }

        //Implemented in mainhub
        public void RemoveItem(int productId,string UserId)
        {
            //The following returns a cartItem object if its exists with the given product Id
            //and cart id
            Cart item = db.Cart.FirstOrDefault(p => p.ProductId == productId && p.UserCartId == UserId);
            //If its exists we delete it from database => Table CartItem
            if (item != null)
            {
                db.Cart.Remove(item);
                db.SaveChanges();
            }
        }
        
        public decimal GetOrderCost(int orderId, string userId )
        {
            //CartItem item = db.CartItem.FirstOrDefault(x => x.OrderID == orderId && x.CartId == cartId);
            //Products product = db.Products.FirstOrDefault(x => x.ProductId == item.ProductId);
            //return product.Price;
            return 0;
        }
    }
}
