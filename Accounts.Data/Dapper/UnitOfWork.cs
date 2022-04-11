using Accounts.Data.Contracts;

namespace Accounts.Data.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository Accounts { get; set ; }

        public UnitOfWork(IAccountRepository repo)
        {
            Accounts = repo;
        }
    }
}
