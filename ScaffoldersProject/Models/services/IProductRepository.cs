﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IProductRepository
    {
        //We use this interface so as to add products either manual either from database either 
        //from another server.

        //Properties
        IQueryable<Products> Products { get; }       

        //Methods 
        Task SaveProduct(Products product);

        Task UpdateProduct(Products product);

        Task<Products> DeleteProduct(int productId);

        Task<decimal> GetCurrentPrice(int productId);

        Task SetCurrentPrice(int productId , decimal closedPrice);
    }
}


//................Difference between IQueryable and IEnumerable..............
//The difference is that IQueryable<T> is the interface that allows LINQ-to-SQL
//(LINQ.-to-anything really) to work.So if you further refine your query on an 
//IQueryable<T>, that query will be executed in the database, if possible.

//For the IEnumerable<T> case, it will be LINQ-to-object, meaning that all objects 
//matching the original query will have to be loaded into memory from the database.

//In code:

//IQueryable<Customer> custs = ...;
// Later on...
//var goldCustomers = custs.Where(c => c.IsGold);
//That code will execute SQL to only select gold customers.The following code, on the 
//other hand, will execute the original query in the database, then filtering out the 
//non-gold customers in the memory:

//IEnumerable<Customer> custs = ...;
// Later on...
//var goldCustomers = custs.Where(c => c.IsGold);
//This is quite an important difference, and working on IQueryable<T> can in many cases 
//save you from returning too many rows from the database.Another prime example is doing 
//paging: If you use Take and Skip on IQueryable, you will only get the number of rows 
//requested; doing that on an IEnumerable<T> will cause all of your rows to be loaded in
//memory.