using Accounts.Model.Registration;

namespace Accounts.Business.Registration
{
    public interface IRegistrationValidator
    {
        public Task<int> Register(RegistrationRequest request);
        public string GenerateStripeId(RegistrationRequest request);
        //public void GenerateCatalog(CatalogItem newCatalog);
    }
}