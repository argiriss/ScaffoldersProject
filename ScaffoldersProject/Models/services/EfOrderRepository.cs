﻿using ScaffoldersProject.Data;
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

        public IQueryable<Order> Orders => db.Order;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfOrderRepository(MainDbContext db)
        {
            this.db = db;
        }
        //method for Adding the new order to Order table
        public void InitializeOrder(Order orderDetails)
        {
            
        }
    }
}
