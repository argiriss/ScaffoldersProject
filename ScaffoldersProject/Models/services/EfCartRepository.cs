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
        public EfCartRepository(MainDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Cart> Cart => db.Cart;
        

        public void AddItem(Products product , int quantity , int CardId)
        {
            CartItem item = db.CartItem.SingleOrDefault(x => x.CartId==CardId);
            if (item == null)
            {
                item = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    CartId = CardId,


                };
                db.CartItem.Update(item);
                db.SaveChanges();
            }
            else
            {
                item.Quantity += quantity;
                db.CartItem.Update(item);
                db.SaveChanges();

            }

         
        }

        public void Clear()
        {
            
        }

        public decimal ComputeTotalCost(int CardId)
        {
            Cart c = db.Cart.SingleOrDefault(x => x.CartId == CardId);
            if (c!=null)
            {
                return c.Items.Sum(x => x.Product.Price * x.Quantity);
            }
            return 0;
        }

        public void RemoveItem(CartItem item)
        {
            throw new NotImplementedException();
        }
    }
}
