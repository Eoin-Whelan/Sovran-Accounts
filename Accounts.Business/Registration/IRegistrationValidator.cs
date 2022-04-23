using Accounts.Model.Registration;

namespace Accounts.Business.Registration
{
    /// <summary>
    /// Contract for registration flow class.
    /// </summary>
    public interface IRegistrationValidator
    {
        public Task<RegistrationResponse> Register(RegistrationRequest request);
        public string GenerateProfileImage(string encodedImage, string username);

    }
}