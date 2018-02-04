using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IAskRepository
    {
        IQueryable<Ask> Asks { get; }
        Task AddAsk(int productId, decimal askAmount, decimal limitPrice, string userId);
        void RemoveAsk();
    }
}
