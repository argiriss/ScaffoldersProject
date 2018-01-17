using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public List<CartItem> Items => items;
        public string UserId; //similar to foreign Key
        public bool Checkout { get; set; }

        private List<CartItem> items = new List<CartItem>();


        public virtual void AddProduct(Products product, int quantity)
        {
            CartItem item = items
                            .Where(x => x.Product.ProductId == product.ProductId)
                            .SingleOrDefault();

            if (item == null)
            {
                items.Add(new CartItem(product, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public virtual void Clear()
        {
            items.Clear();
        }

        public decimal CalculateTotalCost()
        {
            return items.Sum(x => x.Product.Price * x.Quantity);
        }
    }
}
