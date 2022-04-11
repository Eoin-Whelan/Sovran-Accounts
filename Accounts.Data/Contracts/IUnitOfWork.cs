namespace Accounts.Data.Contracts
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; set; }
    }
}
