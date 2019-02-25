using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly IPrinterTonerHandler _printerTonerHandler;
        private readonly ICustomerHandler _customerHandler;

        public HomeController(IUserHandler userHandler,IPrinterTonerHandler printerTonerHandler,ICustomerHandler customerHandler)
        {
            _userHandler = userHandler;
            _printerTonerHandler = printerTonerHandler;
            _customerHandler = customerHandler;
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

        public ActionResult GetTonerLowForCustomer(int customerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers(Session["UserName"].ToString()).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userid = _userHandler.GetUsers(Session["UserName"].ToString()).First().userId;
            return _printerTonerHandler.GetLowTonerLevelsOfCustomerPrinters(customerId, userid);
        }
    }
}