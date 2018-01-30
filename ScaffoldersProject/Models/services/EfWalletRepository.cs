using ScaffoldersProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class EfWalletRepository:IWalletRepository
    {
        public IQueryable<Deposit> Deposits => db.Deposit;

        private MainDbContext db;

        //Depedency Injection
        public EfWalletRepository(MainDbContext db)
        {
            this.db = db;
        }

        //Save to Deposit Table
        public async Task WalletSave(Deposit deposit)
        {
            db.Deposit.Add(deposit);
            await db.SaveChangesAsync();
        }

        //Deposit to my wallet via Paypal
        public async Task Deposit(decimal amount , string userId)
        {
            Deposit depForSave = new Deposit();
            depForSave.DepositAmount += amount;
            depForSave.DepositDate = DateTime.Now;
            depForSave.UserDepositId = userId;

            await WalletSave(depForSave);
            
        }

        public async Task<decimal> TotalInMyWallet(string userId)
        {
            decimal total = 0;
            var userRecordsInWallet = db.Deposit.Where(x => x.UserDepositId == userId).ToList();

            foreach (var item in userRecordsInWallet)
            {
                total += item.DepositAmount;
            }

            return total;
        }

        //pay from my wallet
        public void PayFromWallet(decimal Amount , string UserId)
        {

        }
        

    }
}
