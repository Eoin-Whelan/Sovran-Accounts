
using Accounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Business.Repository
{
    /// <summary>
    /// ECF Repo is an unimplemented, incomplete class used when exploring database
    /// object relational-mapping capabilities. Based on Entity Core Framework.
    /// </summary>
    public class ECFRepo : IRepository<MerchantAccount, int>
    {
        private readonly AccountsContext _context;

        public ECFRepo(AccountsContext context)
        {
            _context = context;
        }

        public MerchantAccount GetById(int id)
        {
            try
            {
                var found = _context.Merchants.Where(s => s.MerchantId == id).FirstOrDefault();
                if (found == null)
                {
                    return null;
                }
                return found;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Insert(MerchantAccount newAccount)
        {
            try
            {
                _context.Merchants.Add(newAccount);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public Task<MerchantAccount> UpdateById(MerchantAccount iem)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            using (_context)
            {
                try
                {
                    if (_context.Database.CanConnect())
                    {
                        return true;
                    }
                }
                catch(Exception ex)
                {
                }
                return false;
            }
        }

        Task<MerchantAccount> IRepository<MerchantAccount, int>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<MerchantAccount> IRepository<MerchantAccount, int>.Insert(MerchantAccount iem)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRepository<T1, T2>
    {
        Task<MerchantAccount> GetById(int id);
        Task<MerchantAccount> Insert(MerchantAccount iem);
    }
}
