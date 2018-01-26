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

        public IQueryable<Order> Orders => db.Order;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfOrderRepository(MainDbContext db)
        {
            this.db = db;
        }

        //method for Adding the new order to Order table
        public void AddNewOrder(Order orderDetails , Cart c)
        {
            //Add the order to Order Table and save it
          
            db.Order.Add(orderDetails);
            db.SaveChanges();

            //Find the cartitems which exist in User's Cart
            var CartItemsOrdered = db.Cart.Where(x => x.UserCartId == c.UserCartId);
            //for those CartItems set the Orderid(fk) to marked as ordered
            foreach (var item in CartItemsOrdered)
            {
                item.OrderID = orderDetails.OrderID;
                //for each product in product table which productId exist in CartItemsOrdered list reduce the stock
                foreach (var prod in db.Products)
                {
                    if (prod.ProductId == item.ProductId)
                    {
                        prod.Stock -= item.Quantity; //reduce the stock
                        db.Products.Update(prod); //update the table
                    }
                }

            }
            db.CartItem.UpdateRange(CartItemsOrdered);
            //Update the Product table by reducing the stock of ordered product respectively

            db.SaveChanges();
        }
    }
}
