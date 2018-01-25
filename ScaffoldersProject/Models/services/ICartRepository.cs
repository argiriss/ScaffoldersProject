using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface ICartRepository
    {
        IQueryable<Cart> Cart { get; }

        void CartSave(Cart cart);
        void AddItem(Products product, int quantity, string userId);
        void RemoveItem(int productId,string userId);
        decimal ComputeTotalCost(string userId);
        void Clear(string userId);
        decimal GetOrderCost(int orderId, string userId);

    }
}
