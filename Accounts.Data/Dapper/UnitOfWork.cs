using Accounts.Data.Contracts;

namespace Accounts.Data.Dapper
{
    /// <summary>
    /// UnitOfWork class used to group all database interactions to a single transaction.
    /// </summary>
    /// <seealso cref="Accounts.Data.Contracts.IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository Accounts { get; set ; }

        public UnitOfWork(IAccountRepository repo)
        {
            Accounts = repo;
        }
    }
}
