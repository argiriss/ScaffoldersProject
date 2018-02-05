using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfOfferRepository:IOfferRepository
    {
        private MainDbContext db;

        public IQueryable<Offer> Offers => db.Offer;

        //Depedency injection implemented on constructor.Instatiate db object
        public EfOfferRepository(MainDbContext db)
        {
            this.db = db;
        }

        public async Task AddOffer(int productId,decimal bidAmount, decimal limitPrice,string userId)
        {
            var sameOffer = db.Offer.SingleOrDefault(x => x.PriceOffer == limitPrice && x.ProductId == productId);
            //if the product already exists with same price then increase the quanity
            if (sameOffer != null)
            {
                sameOffer.Quantity += bidAmount;
                db.Offer.Update(sameOffer);
            }
            //if not initialize a new offer and add to table Offer
            else
            {
                var offer = new Offer
                {
                    Quantity = bidAmount,
                    PriceOffer = limitPrice,
                    DateofOffer = DateTime.Now,
                    UserOfferId = userId,
                    ProductId = productId
                };
                db.Offer.Add(offer);
            }
            await db.SaveChangesAsync();
        }

        public void RemoveOffer()
        {
            throw new NotImplementedException();
        }
    }
}
