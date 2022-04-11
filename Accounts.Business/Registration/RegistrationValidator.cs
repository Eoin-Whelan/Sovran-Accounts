using Accounts.Data.Contracts;
using Accounts.Model;
using Accounts.Model.Registration;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;


namespace Accounts.Business.Registration
{
    public class RegistrationValidator : IRegistrationValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApiProxy _catalogApiProxy;
        private readonly IApiProxy _paymentApiProxy;
        private readonly IImageHandler _imageHandler;
        public RegistrationValidator(IUnitOfWork unitOfWork, IApiProxy apiProxy, IImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _catalogApiProxy = apiProxy;
            _imageHandler = imageHandler;
        }

        public async Task<int> Register(RegistrationRequest request)
        {
            int retrievedId;
            try
            {

                //request.NewAccount = GenerateImageLink(request.NewAccount);
                //request.NewCatalog.ProfileImg = request.NewAccount.ProfileImg;
                //  Send create req to catalog service
                await _catalogApiProxy.CatalogInsertMerchantAsync(request.NewCatalog);

                // Send request to Payment service *here*

                //  Post to sql table
                var result = await _unitOfWork.Accounts.AddAsync(request.NewAccount);

                if (result != 1)
                {
                    throw new Exception("Internal error.");
                }
                retrievedId = await _unitOfWork.Accounts.GetByUsername(request.NewAccount.Username);
            }
            catch(Exception ex)
            {
                retrievedId = -1;
            }
            return retrievedId;

        }

        public string GenerateStripeId(RegistrationRequest request)
        {
            return "";
        }

        public MerchantAccount GenerateImageLink(MerchantAccount request)
        {
            string profileImg = _imageHandler.PostImage(request.ProfileImg, request.Username, "profileImg");
            request.ProfileImg = profileImg;
            return request;
        }
    }
}
