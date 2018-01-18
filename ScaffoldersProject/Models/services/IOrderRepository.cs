using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    interface IOrderRepository
    {
        //Properties
        IQueryable<Order> Orders { get; }

        //Methods 
        //We can add more methods...........

        void SaveOrder(Order order);

        void UpdateOrder(Order order);
     
    }
}
