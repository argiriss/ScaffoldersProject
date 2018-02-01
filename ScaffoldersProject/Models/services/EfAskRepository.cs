using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfAskRepository : IAskRepository
    {
        private MainDbContext db;
        public IQueryable<Ask> Asks => db.Ask;
        //depedency injection
        public EfAskRepository(MainDbContext db)
        {
            this.db = db;
        }

        public async Task AskSave(Ask ask)
        {
            db.Ask.Add(ask);
            await db.SaveChangesAsync();
        }

        public async Task AddAsk(string userId, decimal price, double quantity, int productId)
        {
            var ask = new Ask
            {
                Quantity = quantity,
                PriceAsk = price,
                DateofAsk = DateTime.Now,
                UserAskId = userId,
                ProductId = productId
            };
            await AskSave(ask);
           
        }

        public void RemoveAsk()
        {
            
        }
    }
}
