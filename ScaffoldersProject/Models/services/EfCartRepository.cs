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
            CartItem findProductInCart = db.CartItem.FirstOrDefault(x => x.CartId == cart.CartId);
            if (findProductInCart != null)
            {
                //If item exists then we look for similar productId 
                CartItem findInCart = db.CartItem.FirstOrDefault(x => x.ProductId == product.ProductId && x.CartId == cart.CartId);
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

        public void Clear()
        {

        }

        public decimal ComputeTotalCost(Cart cart)
        {
            decimal totalCost = 0;
            var findListOfItems = db.CartItem.Where(x => x.CartId == cart.CartId);
            foreach (var item in findListOfItems)
            {
                totalCost += item.Product.Price * item.Quantity;
            }
            return totalCost;
        }

        public void RemoveItem(CartItem item)
        {
            throw new NotImplementedException();
        }
    }
}
