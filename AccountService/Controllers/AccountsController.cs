using Accounts.Business.Login;
using Accounts.Business.Registration;
using Accounts.Data.Contracts;
using Accounts.Model.Registration;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sovran.Logger;

namespace AccountsService
{
    public class AccountsController : Controller
    {
        public ILoginRequestValidator _loginValidator;
        private readonly IRegistrationValidator _registrationValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SovranLogger _logger;

        public AccountsController(SovranLogger logger, ILoginRequestValidator loginValidator,  IUnitOfWork unitOfWork, IRegistrationValidator registrationValidator)
        {
            _logger = logger;
            _loginValidator = loginValidator;
            _registrationValidator = registrationValidator;
            _unitOfWork = unitOfWork;
        }

        [Route("RegisterAccount")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountAsync([FromBody]RegistrationRequest newAccount)
        {
            try
            {
                _logger.LogActivity("Registration flow initiated");
                _logger.LogPayload(newAccount);
                var result = await _registrationValidator.Register(newAccount);
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
        /// Test Endpoint for front-end handling. Returns response with google.ie link to mock Stripe onboarding transition.
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [Route("DummyRegister")]
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

        // GET: AccountsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Route("PullById")]
        [HttpPost]
        public ActionResult RetrieveById(int id)
        {
            var DapperResult = _unitOfWork.Accounts.GetByIdAsync(id).Result;

            //var ECFResult = _accountRepo.GetById(id);

            return Ok(DapperResult);
        }

        [Route("RetrieveStripeAccount")]
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

        [Route("NewLogin")]
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var validLogin = await _unitOfWork.Accounts.AttemptLogin(username, password);

            if(validLogin != -1)
            {
                return Ok(validLogin);
            }
            return StatusCode(500);
        }
    }
}
