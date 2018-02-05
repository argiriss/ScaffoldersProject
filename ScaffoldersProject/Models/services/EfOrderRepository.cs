﻿using Microsoft.AspNetCore.Identity;
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

        public async Task InstantOrder(Order instantOrder,int productId,decimal euroSpend)
        {
            //Add the order to Order Table and save it
            await OrderSave(instantOrder);

            //Find current product price
            var product = await db.Products.FindAsync(productId);
           
            //Add to cartOrder table the new order
            CartOrder newCartOrder = new CartOrder
            {
                OrderId = instantOrder.OrderID,
                ProductId = productId,
                Quantity = euroSpend/product.Price
            };

            await CartOrderSave(newCartOrder);

            //if product not exist in portfolio insert it
            var check = db.PortFolio.FirstOrDefault(x => x.ProductId == productId);
            if (check != null)
            {
                //if exists the raise its coin quantity
                check.CoinsQuantity += newCartOrder.Quantity;
            }
            else
            {
                //insert new product to portfolio
                Portfolio newproduct = new Portfolio
                {
                    ProductId = productId,
                    CoinsQuantity = newCartOrder.Quantity
                };
            }

            //Reduse the Wallet amount by euroSpend in parameters
            //Find Client Wallet and reduse it
            var clientUser = await _userManager.FindByIdAsync(instantOrder.UserOrderId);
            clientUser.Wallet -= euroSpend;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);

        }

        public async Task<List<Products>> GetClientOrders(string userId)
        {
            var clientOrders = db.Order.Where(x => x.UserOrderId == userId).ToList();
            var clientOrdersIds = clientOrders.Select(x => x.OrderID).ToList();
            var orders = db.CartOrder.Where(x => clientOrdersIds.Contains(x.OrderId)).ToList();
            var productsIds = orders.Select(x => x.ProductId).Distinct().ToList();
            var orderedProducts = db.Products.Where(x => productsIds.Contains(x.ProductId)).ToList();
            return orderedProducts;
        }

        public decimal ClientSpecificProductTotal(int productId,string userId)
        {
            //find all user orders in order table
            var clientOrders = db.Order.Where(x => x.UserOrderId == userId).ToList();
            var clientOrdersIds = clientOrders.Select(x => x.OrderID).ToList();
            var orders = db.CartOrder.Where(x => clientOrdersIds.Contains(x.OrderId) && x.ProductId==productId);
            var sum = orders.Select(x => x.Quantity).Sum();
            return sum;
        }



        //method for adding the offer to Offer table in database
        //public void AddOffer(string userId, decimal price, double quantity , int productId)
        //{
        //    var sameOffer = db.Offer.Single(x => x.PriceOffer == price && x.ProductId == productId);
        //    //if the product already exists with same price then increase the quanity
        //    if (sameOffer != null)
        //    {
        //        sameOffer.Quantity += quantity;
        //        db.Offer.Update(sameOffer);
        //    }
        //    //if not initialize a new offer and add to table Offer
        //    else
        //    {
        //        var offer = new Offer
        //        {
        //            Quantity = quantity,
        //            PriceOffer = price,
        //            DateofOffer = DateTime.Now,
        //            UserOfferId = userId,
        //            ProductId=productId

        //        };
        //        db.Offer.Add(offer);
        //    }
        //    db.SaveChanges();
        //}

        ////method for adding an ask
        //public void AddAsk(string userId, decimal price, double quantity, int productId)
        //{
        //    var Ask = new Ask
        //    {
        //        Quantity = quantity,
        //        PriceAsk = price,
        //        DateofAsk = DateTime.Now,
        //        UserAskId = userId,
        //        ProductId = productId
        //    };
        //    db.Ask.Add(Ask);
        //    db.SaveChanges();
        //}



    }
}
