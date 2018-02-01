using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IOfferRepository
    {
        IQueryable<Offer> Offers { get; }
        void AddOffer(string userId, decimal price, double quantity, int productId);
        void RemoveOffer();
    }
}
