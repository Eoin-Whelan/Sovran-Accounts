using Accounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Business.Update
{
    /// <summary>
    /// Contract class for UpdateValidator. Used in dependency injection.
    /// </summary>
    public interface IUpdateValidator
    {
        public Task UpdateAccount(MerchantAccount update);
    }
}
