using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfOfferRepository : IOfferRepository
    {
        private MainDbContext db;

        public IQueryable<Offer> Offers => db.Offer;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfOfferRepository(MainDbContext db)
        {
            this.db = db;
        }

        public async Task AddOffer(int productId, decimal bidAmount, decimal limitPrice, string userId)
        {
            //Create new offer in Database
            var offer = new Offer
            {
                Quantity = bidAmount,
                PriceOffer = limitPrice,
                DateofOffer = DateTime.Now,
                UserOfferId = userId,
                ProductId = productId
            };
            db.Offer.Add(offer);
            await db.SaveChangesAsync();

        }

        public void RemoveOfferAsync(Ask itemForRemoval)
        {
            db.Ask.Remove(itemForRemoval);
            db.SaveChanges();
            //TO DO: the offers which removed must increase the revenues of member
        }

        public void ReduceOfferAsync(Ask itemForReduce)
        {
            db.Ask.Update(itemForReduce);
            db.SaveChanges();
            //TO DO: the offers which removed must increase the revenues of member
        }
    }
}
