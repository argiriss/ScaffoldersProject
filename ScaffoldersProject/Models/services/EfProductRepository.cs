using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfProductRepository : IProductRepository
    {
        private MainDbContext db;

        public IQueryable<Products> Products
        {
            get
            {
                return db.Products;
            }
        }
        //writing the above with expression body
        //public IQueryable<Products> Products => db.Products;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfProductRepository(MainDbContext db)
        {
            this.db = db;
        }

        public Products DeleteProduct(int productId)
        {
            //The following returns a products object if its exists
            Products prod = db.Products.FirstOrDefault(p => p.ProductId == productId);
            //If its exists
            if (prod != null)
            {
                db.Products.Remove(prod);
                db.SaveChanges();
            }
            return prod;
        }

        public void SaveProduct(Products product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void UpdateProduct(Products product)
        {
            db.Update(product);
            db.SaveChanges();
        }
    }
}
