using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface ICartRepository
    {
        IQueryable<Cart> Cart { get; }

        Task CartSave(Cart cart);
        Task AddItem(Products product, int quantity, string userId);
        void RemoveItem(int productId,string userId);
        decimal ComputeTotalCost(string userId);
        Task Clear(string userId);
        decimal GetOrderCost(int orderId, string userId);

    }
}
