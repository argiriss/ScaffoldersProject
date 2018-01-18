using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface ICartRepository
    {
        IQueryable<Cart> Cart { get; }

        void AddItem(Products product , int quantity , int cardId);
        void RemoveItem(CartItem item);
        decimal ComputeTotalCost(int cardId);
        void Clear();

    }
}
