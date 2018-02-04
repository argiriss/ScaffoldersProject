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
        
        //this method saves the new ask(aka bid) into Ask table
        public async Task AskSave(Ask ask)
        {
            db.Ask.Add(ask);
            await db.SaveChangesAsync();
        }

        public async Task AddAsk(string userId, decimal price, decimal quantity, int productId)
        {
            //find if a bid with the same product and the same price exists already in Ask table
            var askExist = db.Ask.SingleOrDefault(x => x.ProductId == productId && x.PriceAsk == price);
            //if exists increase the quantity
            if (askExist != null)
            {
                askExist.Quantity += quantity;
                db.Ask.Update(askExist);
                await db.SaveChangesAsync();
            }
            //else initiate a new Ask object and add it to Ask table
            else
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
        }

        public void RemoveAsk()
        {

        }
    }
}
