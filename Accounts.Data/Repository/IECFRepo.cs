using Accounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Repository
{
    /// <summary>
    /// Unused ECF contract interface.
    /// </summary>
    public interface IECFRepo : IDisposable
    {
        bool CreateAccount(MerchantAccount newAccount);

        bool Login(string username, string password);
        MerchantAccount UpdateAccount(MerchantAccount newDetails);
        MerchantAccount GetById(int id);
        public bool IsConnected();


        bool ChangePassword(string oldPassword, string newPassword);

        bool UpdateCarousel(List<string> passedNames);
    }
}
