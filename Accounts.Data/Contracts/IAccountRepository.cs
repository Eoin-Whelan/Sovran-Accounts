using Accounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Contracts
{
    public interface IAccountRepository
    {
        Task<MerchantAccount> GetByIdAsync(int id);
        Task<int> GetByUsername(string username);

        Task<IReadOnlyList<MerchantAccount>> GetAllAsync();
        int AddMerchant(MerchantAccount entity);
        Task<int> UpdateAsync(MerchantAccount entity);
        Task<int> DeleteAsync(int id);
        Task<int> AttemptLogin(string username, string password);
        Task<string> GetStripeAccount(string username);

    }
}
