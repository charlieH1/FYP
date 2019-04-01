using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TonerManagement.Controllers
{
    public class DevicesController : Controller
    {
        // GET: Devices
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDeviceDetails()
        {
            //ToDo implement fetching details in such a way where props are success customer and printer
            throw new NotImplementedException();
        }

        public ActionResult GetTonerPercentage()
        {
            //ToDo implement fetching current toner percentage
            throw new NotImplementedException();
        }
    }
}