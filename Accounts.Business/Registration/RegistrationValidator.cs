using Accounts.Data.Contracts;
using Accounts.Model;
using Accounts.Model.Registration;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;
using AccountService.ServiceClients.Payment.ApiProxy;
using Sovran.Logger;

namespace Accounts.Business.Registration
{
    /// <summary>
    /// Primary handler for registration flow. Communicates with Catalog and Payment<br></br> services to
    /// generate necessary details for Merchant account creation.
    /// </summary>
    public class RegistrationValidator : IRegistrationValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICatalogProxy _catalogApiProxy;
        private readonly IPaymentProxy _paymentApiProxy;
        private readonly IImageHandler _imageHandler;
        private readonly SovranLogger _logger;
        /// <summary>
        /// Primary constructor. Utilizes dependency injection to avoid concrete instantiations in main controller.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="catalogProxy"></param>
        /// <param name="paymentProxy"></param>
        /// <param name="imageHandler"></param>
        /// <param name="logger"></param>
        public RegistrationValidator(IUnitOfWork unitOfWork, ICatalogProxy catalogProxy, IPaymentProxy paymentProxy, IImageHandler imageHandler, SovranLogger logger)
        {
            _unitOfWork = unitOfWork;
            _catalogApiProxy = catalogProxy;
            _paymentApiProxy = paymentProxy;
            _imageHandler = imageHandler;
            _logger = logger;
        }

        /// <summary>
        /// Primary registration flow method. ApiProxy's are communicated with, culminating in a merchant catalog and Stripe account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            // Instantiate a container response for front-end.
            var registrationResponse = new RegistrationResponse();

            var stripeRequest = new PaymentRegistrationRequest
            {
                EmailAddress = request.NewAccount.MerchantEmail
                };

            try
            {
                //  If username is unique
                if (_unitOfWork.Accounts.DoesExist(request.NewAccount.Username))
                {

                    _logger.LogActivity("Cloudinary flow began.");
                    // Generate image links for the profile and product images
                    if (request.NewAccount.ProfileImg != null)
                    {
                        string profileImgUrl = GenerateProfileImage(request.NewAccount.ProfileImg, request.NewAccount.Username);
                        request.NewAccount.ProfileImg = profileImgUrl;
                        request.NewCatalog.ProfileImg = profileImgUrl;
                    }
                    if (request.NewCatalog.Catalog.First<CatalogItem>().ItemImg != null)
                    {
                        string imgUrl = request.NewCatalog.Catalog.First<CatalogItem>().ItemImg;
                        request.NewCatalog.Catalog.First<CatalogItem>().ItemImg =
                            _imageHandler.PostProductImg(imgUrl, request.NewAccount.Username, request.NewCatalog.Catalog.First().ItemName);
                    }
                    _logger.LogActivity("Cloudinary flow complete. Beginning catalog proxy flow.");



                    //  Send async req to generate a new merchant catalog
                    await _catalogApiProxy.CatalogInsertMerchantAsync(request.NewCatalog);

                    _logger.LogActivity("Catalog proxy flow complete. Beginning payment proxy flow.");



                    // Send request to payment service for Stripe onboarding
                    var paymentResponse = await _paymentApiProxy.RegisterAccountAsync(stripeRequest);
                    _logger.LogActivity("Catalog proxy flow complete. Beginning payment proxy flow.");
                    _logger.LogPayload(paymentResponse);

                    registrationResponse.stripeOnBoardingUrl = paymentResponse.OnboardingUrl;
                    request.NewAccount.StripeId = paymentResponse.StripeAccountNo;
                    _logger.LogActivity("Payment proxy flow complete. Beginning SQL insertion.");

                    var result = _unitOfWork.Accounts.AddMerchant(request.NewAccount);
                    if (result != 1)
                    {
                        _logger.LogError("Registration failed. Username:" + request.NewAccount.Username);
                        registrationResponse.result = false;
                    }
                    else
                    {
                        _logger.LogError("Registration complete for Username:" + request.NewAccount.Username);
                    }
                }
            }
            catch (Exception ex)
            {
                //  Swallow exception and log exception and payload.
                _logger.LogError("Exception caught: " + ex.Message);
                _logger.LogPayload(registrationResponse);
                registrationResponse.errorMsg = "Internal server error occured. Please contact system administrator.";
                registrationResponse.result = false;
            }
            return registrationResponse;
        }

        /// <summary>
        /// Genera
        /// </summary>
        /// <param name="encodedImage"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GenerateProfileImage(string encodedImage, string username)
        {
            string profileImg = _imageHandler.PostProfileImg(encodedImage, username);
            return profileImg;
        }
    }
}
