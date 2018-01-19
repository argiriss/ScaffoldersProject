using Microsoft.EntityFrameworkCore;
using ScaffoldersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Data
{
    public class MainDbContext:DbContext
    {
        //Database for all the other uses ,Products,orders etc.....
        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
    
        //We can add other models ............

        //constructor 
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
    }
}

//How to add-migration when i have two db contexts
//Add-Migration InitialCreate -Context MainDbContext -OutputDir Data/Migrations
//update-database -context MainDbContext