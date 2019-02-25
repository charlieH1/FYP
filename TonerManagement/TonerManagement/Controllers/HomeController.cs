using System.Linq;
using System.Net;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly IPrinterTonerHandler _printerTonerHandler;

        public HomeController(IUserHandler userHandler,IPrinterTonerHandler printerTonerHandler)
        {
            _userHandler = userHandler;
            _printerTonerHandler = printerTonerHandler;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCoverage(CoverageForCompanyRequestModel request)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers(Session["UserName"].ToString()).Count==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = _userHandler.GetUsers(Session["UserName"].ToString()).First().userId;
            return _printerTonerHandler.GetCoverage(request, userId);

        }
    }
}