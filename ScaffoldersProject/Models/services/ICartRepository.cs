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
        void AddItem(Products product, int quantity, Cart cart);
        void RemoveItem(CartItem item);
        decimal ComputeTotalCost(Cart cart);
        void Clear();

    }
}
