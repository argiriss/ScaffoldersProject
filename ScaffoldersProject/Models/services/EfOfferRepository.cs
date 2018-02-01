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

        public void AddOffer(string userId, decimal price, double quantity, int productId)
        {
            var sameOffer = db.Offer.Single(x => x.PriceOffer == price && x.ProductId == productId);
            //if the product already exists with same price then increase the quanity
            if (sameOffer != null)
            {
                sameOffer.Quantity += quantity;
                db.Offer.Update(sameOffer);
            }
            //if not initialize a new offer and add to table Offer
            else
            {
                var offer = new Offer
                {
                    Quantity = quantity,
                    PriceOffer = price,
                    DateofOffer = DateTime.Now,
                    UserOfferId = userId,
                    ProductId = productId

                };
                db.Offer.Add(offer);
            }
            db.SaveChanges();
        }

        public void RemoveOffer()
        {
            throw new NotImplementedException();
        }
    }
}
