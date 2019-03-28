using System;
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
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }

        

        public ActionResult GetTonerLowForCustomer(int customerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userid = _userHandler.GetUsers((string)Session["UserName"]).First().userId;
            return _printerTonerHandler.GetLowTonerLevelsOfCustomerPrinters(customerId, userid);
        }
    }
}