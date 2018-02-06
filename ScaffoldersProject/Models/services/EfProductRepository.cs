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

        public async Task<Products> DeleteProduct(int productId)
        {
            //The following returns a products object if its exists
            Products prod = db.Products.FirstOrDefault(p => p.ProductId == productId);
            //If its exists
            if (prod != null)
            {
                db.Products.Remove(prod);
                await db.SaveChangesAsync();
            }
            return prod;
        }

        public async Task SaveProduct(Products product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
        }

        public async Task UpdateProduct(Products product)
        {
            db.Update(product);
            await db.SaveChangesAsync();
        }

        public async Task<decimal> GetCurrentPrice(int productId)
        {
            var product = await db.Products.FindAsync(productId);
            return product.Price;
        }
        //method for setting the new price which comes from a instant buy or an instant sell
        public async Task SetCurrentPrice(int productId , decimal closedPrice)
        {
            var product = await db.Products.FindAsync(productId);
            product.Price = closedPrice;
            db.Products.Update(product);
            await db.SaveChangesAsync();
        }
    }
}
