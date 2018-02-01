using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IAskRepository
    {
        IQueryable<Ask> Asks { get; }
        Task AddAsk(string userId, decimal price, double quantity, int productId);
        void RemoveAsk();
    }
}
