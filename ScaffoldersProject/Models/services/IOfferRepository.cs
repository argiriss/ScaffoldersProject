using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IOfferRepository
    {
        IQueryable<Offer> Offers { get; }
        Task AddOffer(int productId, decimal bidAmount, decimal limitPrice, string userId);
        void RemoveOffer();
    }
}
