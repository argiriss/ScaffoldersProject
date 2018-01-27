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

                //!!!!!!Inside foreach no Iquerable thats why i made CartItemsOrdered 
                //To List!!!!!!!!.........Important...............

                //Creation of cartOrder ID in CartOrder Table for transfer items from cart 
                //to cartOrder so as to clear cart table
                cartOrderTable.OrderId = orderDetails.OrderID;
                cartOrderTable.ProductId = item.ProductId;
                cartOrderTable.Quantity = item.Quantity;

                await CartOrderSave(cartOrderTable);

                //Remove item from cart table so as to clear this table after order placed
                db.Cart.Remove(item);

                //for each item removing,  reduce its stock by the buying quantity
                Products productReduseStock = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                productReduseStock.Stock -= item.Quantity; //reduce the stock
                await ProductReduseStock(productReduseStock);

            }//End of foreach loop in cart table


        }
    }
}
