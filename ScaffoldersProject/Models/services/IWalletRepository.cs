using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public interface IWalletRepository
    {
        //Properties
        IQueryable<Deposit> Deposits { get; }

        Task Deposit(decimal Amount, string UserId);

        Task<decimal> TotalInMyWallet(string userId);

        List<Deposit> GetDepositHistory();

        List<Deposit> GetDepositHistory(string userId);

        Task IncreaseWallet(decimal amount, string userId);

        Task DecreaseWallet(decimal amount, string userId);
    }
}
