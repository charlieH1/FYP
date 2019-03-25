using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;

namespace TonerManagement.Controllers
{
    public class StockLocationController : Controller
    {
        private readonly IUserHandler _userHandler;
        public StockLocationController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }
        // GET: StockLocation
        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }


    }
}