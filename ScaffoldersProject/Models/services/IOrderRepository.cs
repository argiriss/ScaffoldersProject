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
        void AddNewOrder(Order orderDetails  , Cart c);

        //void SaveOrder(Order order);

        //void UpdateOrder(Order order);
     
    }
}
