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
        Task InstantOrder(Order instantOrder, int productId, decimal euroSpend);
        Task<List<Products>> GetAllApprovedProducts();
         decimal ClientSpecificProductTotal(int productId, string userId);
        Task UpdateTradeHistory(TradeHistory trade);
        List<TradeHistory> GetTradeHistory(int productId);
        Task SetCurrentPrice(int productId, decimal closedPrice);
        Task<decimal> RealTimePrice(int productId, decimal eurospend);
        Task<decimal> GetProductCuurentPrice(int productId);

        List<CartOrder> GetHistory(string UserId);
        Task Checkout(Order orderDetails);
        //methods for my order book 


        //void SaveOrder(Order order);

        //void UpdateOrder(Order order);

    }
}
