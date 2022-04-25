using Accounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Contracts
{
    /// <summary>
    /// AccountRepository contract/interface. Used in dependency injection method.
    /// </summary>
    public interface IAccountRepository
    {
        Task<MerchantAccount> GetByIdAsync(int id);
        Task<MerchantAccount> GetByUsername(string username);
        int AddMerchant(MerchantAccount entity);
        Task<int> UpdateAsync(MerchantAccount entity);
        Task<int> AttemptLogin(string username, string password);
        Task<string> GetStripeAccount(string username);
        Task<bool> DoesExist(string username);
    }
}
