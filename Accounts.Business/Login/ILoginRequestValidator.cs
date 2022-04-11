namespace Accounts.Business.Login
{
    public interface ILoginRequestValidator
    {
        public bool AttemptLogin();
    }
}