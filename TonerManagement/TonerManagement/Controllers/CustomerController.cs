using System.Net;
using System.Web.Mvc;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerHandler _customerHandler;
        private readonly IUserHandler _userHandler;
        public CustomerController(ICustomerHandler customerHandler, IUserHandler userHandler)
        {
            _customerHandler = customerHandler;
            _userHandler = userHandler;
        }

        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }

        public ActionResult GetCustomersForUser()
        {
            return Session["UserName"] == null ? new HttpStatusCodeResult(HttpStatusCode.Unauthorized) : _customerHandler.GetCustomersForUser((string)Session["UserName"]);
        }

        public ActionResult GetCustomer(int customerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return _customerHandler.GetCustomer(customerId,(string)Session["UserName"]);
        }

        public ActionResult UpdateCustomer(UpdateCustomerModel request)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _customerHandler.UpdateCustomer(request, (string) Session["UserName"]);
            
        }
    }
}