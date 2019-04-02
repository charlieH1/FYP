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
    public class DevicesController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly IDevicesHandler _devicesHandler;
        private readonly IPrinterTonerHandler _printerTonerHandler;

        public DevicesController(IUserHandler userHandler,IDevicesHandler devicesHandler, IPrinterTonerHandler printerTonerHandler)
        {
            _userHandler = userHandler;
            _devicesHandler = devicesHandler;
            _printerTonerHandler = printerTonerHandler;
        }
        // GET: Devices
        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }

        public ActionResult GetDeviceDetails(int printerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _devicesHandler.GetDeviceDetails(printerId, (string) Session["UserName"]);
        }

        public ActionResult GetTonerPercentage(int printerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _printerTonerHandler.GetCurrentTonerLevel(printerId, (string)Session["UserName"]);
        }

        public ActionResult UpdateTonerLow(TonerPercentageAndPrinterIdModel request)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _devicesHandler.UpdateTonerLowOnDevice(request, (string) Session["UserName"]);
        }
    }
}