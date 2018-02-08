using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private MainDbContext db;

        //Depedency Injection
        public EfWalletRepository(MainDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        //Save to Deposit Table
        public async Task DepositSave(Deposit deposit)
        {
            db.Deposit.Add(deposit);
            await db.SaveChangesAsync();
        }

        //Deposit to my wallet via Paypal
        public async Task Deposit(decimal amount , string userId)
        {
            decimal fee = 0.05M;
            decimal amountWithFee = amount-fee*amount;
            //Create new deposit 
            Deposit depForSave = new Deposit();
            depForSave.DepositAmount= amountWithFee;
            depForSave.DepositDate = DateTime.Now;
            depForSave.UserDepositId = userId;

            await DepositSave(depForSave);

            //Find Client Wallet and add the new Value
            var clientUser = await _userManager.FindByIdAsync(userId);
            clientUser.Wallet += amountWithFee;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);

            //Find user Admin to change his wallet with the fee from transaction
            var userToChange=await _userManager.FindByNameAsync("Admin");
            userToChange.Wallet += fee*amount;
            // Save the changes
            await _userManager.UpdateAsync(userToChange);
            
        }

        public async Task<decimal> TotalInMyWallet(string userId)
        {
            //find user by id
            var clientUser = await _userManager.FindByIdAsync(userId);

            return clientUser.Wallet;
        }


        public List<Deposit> GetDepositHistory()
        {
            var deposits = db.Deposit.ToList();
            return deposits;
        }

        public List<Deposit> GetDepositHistory(string userId)
        {
            var UserDeposits = db.Deposit.Where(x=>x.UserDepositId==userId).ToList();
            return UserDeposits;
        }

        public async Task IncreaseWallet(decimal amount, string userId)
        {
            var clientUser = await _userManager.FindByIdAsync(userId);
            clientUser.Wallet += amount;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);
            await db.SaveChangesAsync();
        }

        public async Task DecreaseWallet(decimal amount , string userId)
        {
            var clientUser = await _userManager.FindByIdAsync(userId);
            clientUser.Wallet -= amount;
            //Save the changes
            await _userManager.UpdateAsync(clientUser);
            await db.SaveChangesAsync();
        }


    }
}
