using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;

namespace TonerManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerHandler _customerHandler;
        public CustomerController(ICustomerHandler customerHandler)
        {
            _customerHandler = customerHandler;
        }
        public ActionResult GetCustomersForUser()
        {
            return Session["UserName"] == null ? new HttpStatusCodeResult(HttpStatusCode.Unauthorized) : _customerHandler.GetCustomersForUser((string)Session["UserName"]);
        }
        
    }
}