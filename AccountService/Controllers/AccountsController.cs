using Accounts.Business.Login;
using Accounts.Business.Registration;
using Accounts.Data.Contracts;
using Accounts.Model.Registration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountsService
{
    public class AccountsController : Controller
    {
        public ILoginRequestValidator _loginValidator;
        private readonly ILogger<AccountsController> _logger;
        private readonly IRegistrationValidator _registrationValidator;
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(ILogger<AccountsController> logger, ILoginRequestValidator loginValidator,  IUnitOfWork unitOfWork, IRegistrationValidator registrationValidator)
        {
            _logger = logger;
            _loginValidator = loginValidator;
            _registrationValidator = registrationValidator;
            _unitOfWork = unitOfWork;
        }


        //[FromBody] form
        //[EnableCors("Dev")]
        [Route("RegisterAccount")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountAsync([FromBody]RegistrationRequest newAccount)
        {
            try
            {
                var result = await _registrationValidator.Register(newAccount);

                if(result != -1)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Internal error occurred.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Payment validation threw an exception : ", ex);

                JsonResult result = new JsonResult("Error:" + ex.Message);
                return (result);
            }
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
