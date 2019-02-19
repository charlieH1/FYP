using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class RegistrationController : Controller
    {

        private readonly IRegistrationHandler _registrationHandler;

        public RegistrationController(IRegistrationHandler registrationHandler)
        {
            _registrationHandler = registrationHandler;
        }

        // GET: Registration
        public ActionResult Register()
        {
            return View("~/Views/Registration/Register.cshtml");
        }

        public ActionResult RegistrationRequest(UserRegisterModel userToRegister)
        {
            return _registrationHandler.RegisterUser(userToRegister);
        }
    }
}