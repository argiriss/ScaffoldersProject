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

        public async Task AddAsk(int productId, decimal askAmount, decimal limitPrice,string userId)
        {
            //find if a bid with the same product and the same price exists already in Ask table
            var askExist = db.Ask.FirstOrDefault(x => x.PriceAsk == limitPrice && x.ProductId == productId);
            //if exists increase the quantity
            if (askExist != null)
            {
                askExist.Quantity += askAmount;
                db.Ask.Update(askExist);
            }
            //else initiate a new Ask object and add it to Ask table
            else
            {
                var ask = new Ask
                {
                    Quantity = askAmount,
                    PriceAsk = limitPrice,
                    DateofAsk = DateTime.Now,
                    UserAskId = userId,
                    ProductId = productId
                };
                db.Ask.Add(ask);
            }
            await db.SaveChangesAsync();

        }

        public void RemoveAsk()
        {

        }
    }
}
