using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Contracts
{
    public interface IRepository<T1, T2> where T1: class
    {
        Task<T1> GetById(T2 id);
        Task<T1> Insert(T1 iem);
        Task<T1> UpdateById(T1 iem);
        Task Save();
        public bool IsConnected();
    }
}
