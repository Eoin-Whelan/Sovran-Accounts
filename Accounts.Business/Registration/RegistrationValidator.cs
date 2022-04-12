using Accounts.Data.Contracts;
using Accounts.Model;
using Accounts.Model.Registration;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;
using AccountService.ServiceClients.Payment.ApiProxy;

namespace Accounts.Business.Registration
{
    public class RegistrationValidator : IRegistrationValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICatalogProxy _catalogApiProxy;
        private readonly IPaymentProxy _paymentApiProxy;
        private readonly IImageHandler _imageHandler;
        public RegistrationValidator(IUnitOfWork unitOfWork, ICatalogProxy apiProxy, IImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _catalogApiProxy = apiProxy;
            _imageHandler = imageHandler;
        }

        public async Task<RegistrationResponse> Register(Model.Registration.RegistrationRequest request)
        {
            RegistrationResponse response = null;
            try
            {
                //  Generate image associated with account for 
                request.NewAccount = GenerateImageLink(request.NewAccount);
                request.NewCatalog.ProfileImg = request.NewAccount.ProfileImg;
                //  Send create req to catalog service
                await _catalogApiProxy.CatalogInsertMerchantAsync(request.NewCatalog);

                // Send request to Payment service *here*

                //  Post to sql table
                var result = _unitOfWork.Accounts.AddMerchant(request.NewAccount);
                //retrievedId = await _unitOfWork.Accounts.GetByUsername(request.NewAccount.Username);
            }
            catch(Exception ex)
            {
                response.errorMsg = "Internal server error occured. Please contact system administrator.";
                response.result = false;
            }
            return response;
        }
        public MerchantAccount GenerateImageLink(MerchantAccount request)
        {
            string profileImg = _imageHandler.PostImage(request.ProfileImg, request.Username, "profileImg");
            request.ProfileImg = profileImg;
            return request;
        }
    }
}
