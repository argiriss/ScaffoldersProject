using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfWalletRepository
    {
        private MainDbContext db;
        public IQueryable<Deposit> Orders => db.Deposit;

        public EfWalletRepository(MainDbContext db)
        {
            this.db = db;
        }
        //Deposit to my wallet via Paypal
        public void Deposit(decimal Amount , string UserId)
        {            
        }

        //pay from my wallet
        public void PayFromWallet(decimal Amount , string UserId) { }

    }
}
