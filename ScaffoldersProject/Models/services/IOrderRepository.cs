using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IOrderRepository
    {
        //Properties
        IQueryable<Order> Orders { get; }
        //Methods 
        //We can add more methods...........
        Task AddNewOrder(Order orderDetails);
        Task InstantOrder(Order instantOrder, int productId, decimal euroSpend, decimal quantity);
        Task<List<Products>> GetClientOrders(string userId);
         decimal ClientSpecificProductTotal(int productId, string userId);
        //methods for my order book 


        //void SaveOrder(Order order);

        //void UpdateOrder(Order order);

    }
}
