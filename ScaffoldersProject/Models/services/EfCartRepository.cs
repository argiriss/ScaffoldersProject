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

        public void CartSave(Cart cart)
        {
            db.Cart.Add(cart);
            db.SaveChanges();
        }

        public void AddItem(Products product, int quantity, Cart cart)
        {
            //If we find a cartId from this user
            CartItem findProductInCart = db.CartItem.FirstOrDefault(x => x.CartId == cart.CartId && (x.OrderID == 0 || x.OrderID == null));
            if (findProductInCart != null)
            {
                //If item exists then we look for similar productId 
                CartItem findInCart = db.CartItem.FirstOrDefault(x => x.ProductId == product.ProductId && x.CartId == cart.CartId && (x.OrderID == 0 || x.OrderID == null));
                if (findInCart != null)
                {
                    findInCart.Quantity += quantity;
                    db.CartItem.Update(findInCart);
                    db.SaveChanges();
                }
                else
                {
                    //If there is not similar product then
                    findInCart = new CartItem
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        CartId = cart.CartId,
                    };
                    db.CartItem.Add(findInCart);
                    db.SaveChanges();
                }
            }
            else
            {
                findProductInCart = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    CartId = cart.CartId,
                };
                db.CartItem.Add(findProductInCart);
                db.SaveChanges();
            }
        }
        //Clear all rpoducts from cart
        public void Clear(Cart c)
        {   
            var CartItemsRemove = db.CartItem.Where(x => x.CartId == c.CartId);
            foreach (var item in CartItemsRemove)
            {
                item.CartId = 0;
                db.CartItem.Update(item);
            }
            db.SaveChanges();
        }


        public decimal ComputeTotalCost(Cart cart)
        {
            decimal totalCost = 0;
            //find list of items with same card id
            var findListOfItems = db.CartItem.Where(x => x.CartId == cart.CartId &&(x.OrderID==0 ||x.OrderID==null));
            foreach (var item in findListOfItems)
            {
                decimal price = db.Products.First(x => x.ProductId == item.ProductId).Price;
                totalCost += price * item.Quantity;
            }
            return totalCost;
        }

        public void RemoveItem(int productId,int cartId)
        {
            //The following returns a cartItem object if its exists with the given product Id 
            //and cart id
            CartItem item = db.CartItem.FirstOrDefault(p => p.ProductId == productId && p.CartId==cartId);
            //If its exists we delete it from database => Table CartItem
            if (item != null)
            {
                db.CartItem.Remove(item);
                db.SaveChanges();
            }
        }
        
        public decimal GetOrderCost(int orderId, int cartId )
        {
            CartItem item = db.CartItem.FirstOrDefault(x => x.OrderID == orderId && x.CartId == cartId);
            Products product = db.Products.FirstOrDefault(x => x.ProductId == item.ProductId);
            return product.Price;
        }
    }
}
