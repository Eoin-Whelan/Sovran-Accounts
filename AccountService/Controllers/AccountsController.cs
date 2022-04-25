using Accounts.Business.Registration;
using Accounts.Business.Update;
using Accounts.Data.Contracts;
using Accounts.Model;
using Accounts.Model.Login;
using Accounts.Model.Registration;
using Microsoft.AspNetCore.Mvc;
using Sovran.Logger;

namespace AccountService.Controllers
{
    /// <summary>
    /// Main controller for account service API.
    /// </summary>
    public class AccountsController : Controller
    {
        private readonly IRegistrationValidator _registrationValidator;
        private readonly IUpdateValidator _updateValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISovranLogger _logger;

        public AccountsController(ISovranLogger logger, IUnitOfWork unitOfWork, IUpdateValidator updateValidator, IRegistrationValidator registrationValidator)
        {
            _logger = logger;
            _registrationValidator = registrationValidator;
            _updateValidator = updateValidator;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Primary registration flow. Required encoded image string for profileImg and initial Catalog item image. Otherwise, leave null.
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [Route("/Account/RegisterAccount")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountAsync([FromBody] RegistrationRequest newAccount)
        {
            try
            {
                _logger.LogActivity("Registration flow initiated");
                _logger.LogPayload(newAccount);
                var result = await _registrationValidator.Register(newAccount);
                if (!result.result)
                {
                    _logger.LogPayload(result);
                    _logger.LogError("Registration flow failed. Reason: " + result.errorMsg);
                }
                _logger.LogActivity("Registration flow Complete");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration Failure {ex.Message}");
                JsonResult result = new JsonResult("Internal Error Has Occurred.");
                return BadRequest(-1);
            }
        }
        /// <summary>
        /// Test Endpoint for front-end handling. Returns response with google.ie link
        /// to mock Stripe onboarding transition.
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [Route("/Account/DummyRegister")]
        [HttpPost]
        public async Task<IActionResult> DummyRegister([FromBody] RegistrationRequest newAccount)
        {
            RegistrationResponse reply = new RegistrationResponse
            {
                stripeOnBoardingUrl = "http://www.example.com",
                result = true,
            };
            return Ok(reply);
        }

        /// <summary>
        /// Updates a given merchant based on their Id.
        /// to mock Stripe onboarding transition.
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [Route("/Account/UpdateAccount")]
        [HttpPost]
        public async Task<IActionResult> UpdateAccount([FromBody] MerchantAccount newAccount)
        {
            try
            {
                _logger.LogActivity("Update flow initiated for " + newAccount.Username);
                _logger.LogPayload(newAccount);

                var result = _updateValidator.UpdateAccount(newAccount);
                if (result.IsCompleted)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error encountered in update flow Ex: " + ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Pulls account by Id. Currently unused.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Account/PullById")]
        [HttpPost]
        public ActionResult RetrieveById(int id)
        {
            var DapperResult = _unitOfWork.Accounts.GetByIdAsync(id).Result;
            return Ok(DapperResult);
        }

        /// <summary>
        /// Pulls account by username. Used in account update flow.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("/Account/PullByUsername")]
        [HttpPost]
        public ActionResult PullByUsername(string username)
        {
            try
            {
                var result = _unitOfWork.Accounts.GetByUsername(username).Result;
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieves stripe account based on passed username. May be used in payment flow.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [Route("/Account/RetrieveStripeAccount")]
        [HttpPost]
        public async Task<ActionResult> RetrieveStripeAccount(string username)
        {
            try
            {
                var stripeId = _unitOfWork.Accounts.GetStripeAccount(username).Result;
                return Ok(stripeId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //var ECFResult = _accountRepo.GetById(id);

        }

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [Route("/Login")]
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var validLogin = _unitOfWork.Accounts.AttemptLogin(loginRequest.Username, loginRequest.Password).Result;

            if (validLogin != -1)
            {
                return Ok(validLogin);
            }
            return StatusCode(500);
        }
    }
}
