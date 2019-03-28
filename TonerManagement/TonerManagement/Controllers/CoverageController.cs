using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class CoverageController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly IPrinterTonerHandler _printerTonerHandler;

        public CoverageController(IUserHandler userHandler, IPrinterTonerHandler printerTonerHandler)
        {
            
            _userHandler = userHandler;
            _printerTonerHandler = printerTonerHandler;
        }
        // GET: Coverage
        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }
        public ActionResult GetCoverage(CoverageForCompanyRequestModel request)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = _userHandler.GetUsers((string)Session["UserName"]).First().userId;
            return _printerTonerHandler.GetCoverage(request, userId);

        }

        public ActionResult GetGridCoverageForCustomer(int customerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = _userHandler.GetUsers((string)Session["UserName"]).First().userId;
            return _printerTonerHandler.GetCoverageGridForCustomer(userId, customerId);
        }

        public ActionResult GetCoverageForPrinter(CoverageForPrinterRequestModel request)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = _userHandler.GetUsers((string)Session["UserName"]).First().userId;
            return _printerTonerHandler.GetCoverageForPrinter(request, userId);
        }
    }
}