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
        public DbSet<ClientOrders> ClientOrder { get; set; }
        //We can add other models ............

        //constructor 
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
    }
}
