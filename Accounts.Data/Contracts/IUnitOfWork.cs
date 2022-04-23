namespace Accounts.Data.Contracts
{
    /// <summary>
    /// UnitOfWork contract/interface. Used in dependency injection method.
    /// </summary>
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; set; }
    }
}
