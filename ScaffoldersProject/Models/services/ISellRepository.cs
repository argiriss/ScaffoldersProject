using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface ISellRepository
    {
        //Properties
        IQueryable<Sell> Sells { get; }
        //Methods 
        //We can add more methods...........
        //Task AddNewSell(Sell sellDetails);
        Task InstantSell(Sell instantSell, int productId, decimal euroSpend);
        Task<List<Products>> GetClientSells(string userId);
        decimal ClientSpecificProductTotal(int productId, string userId);
    }
}
