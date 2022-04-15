using Accounts.Model.Registration;

namespace Accounts.Business.Registration
{
    public interface IRegistrationValidator
    {
        public Task<RegistrationResponse> Register(RegistrationRequest request);
        public string GenerateImageLink(string encodedImage, string username, string type);

    }
}