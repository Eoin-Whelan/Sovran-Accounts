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
        public RegistrationValidator(IUnitOfWork unitOfWork, ICatalogProxy catalogProxy, IPaymentProxy paymentProxy, IImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _catalogApiProxy = catalogProxy;
            _paymentApiProxy = paymentProxy;
            _imageHandler = imageHandler;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var registrationResponse = new RegistrationResponse();

            var stripeRequest = new PaymentRegistrationRequest
            {
                EmailAddress = request.NewAccount.MerchantEmail
                };
            
            try
            {
                //  Generate image associated with account for 
                if(request.NewAccount.ProfileImg != null)
                {
                    string profileImgUrl = GenerateImageLink(request.NewAccount.ProfileImg, request.NewAccount.Username, "profileImg");
                    request.NewAccount.ProfileImg = profileImgUrl;
                    request.NewCatalog.ProfileImg = profileImgUrl;
                }
                if(request.NewCatalog.Catalog.First<CatalogItem>().ItemImg != null)
                {
                    string imgUrl = request.NewCatalog.Catalog.First<CatalogItem>().ItemImg;
                    request.NewCatalog.Catalog.First<CatalogItem>().ItemImg = GenerateImageLink(imgUrl, request.NewAccount.Username, "catalog");
                }
                //  Send create req to catalog service
                await _catalogApiProxy.CatalogInsertMerchantAsync(request.NewCatalog);

                // Send request to Payment service *here*
                var paymentResponse = await _paymentApiProxy.RegisterAccountAsync(stripeRequest);

                registrationResponse.stripeOnBoardingUrl = paymentResponse.OnboardingUrl;
                request.NewAccount.StripeId = paymentResponse.StripeAccountNo;

                //  Post to sql table
                var result = _unitOfWork.Accounts.AddMerchant(request.NewAccount);
                if(result != 1)
                {
                    return null;
                }
                registrationResponse.result = true;
            }
            catch(Exception ex)
            {
                registrationResponse.errorMsg = "Internal server error occured. Please contact system administrator.";
                registrationResponse.result = false;
            }
            return registrationResponse;
        }
        public string GenerateImageLink(string encodedImage, string username, string type)
        {
            string profileImg = _imageHandler.PostImage(encodedImage, username, type);
            return profileImg;
        }
    }
}
