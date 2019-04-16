using System.Linq;
using System.Net;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class CustomerHandler : ICustomerHandler
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IUserRepo _userRepo;

        public CustomerHandler(ICustomerRepo customerRepo, IUserRepo userRepo)
        {
            _customerRepo = customerRepo;
            _userRepo = userRepo;
        }

        public ActionResult GetCustomersForUser(string userName)
        {
            //get user
            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var user = users.First();
            var customers = _customerRepo.GetCustomersForUser(user.userId);
            var json = new JsonResult()
            {
                Data = customers,
                ContentEncoding = null,
                ContentType = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return json;
        }

        public ActionResult GetCustomer(int customerId, string userName)
        {
            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var customer = _customerRepo.GetCustomer(customerId);
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound,"Customer Not Found");
            }
            var user = users.First();
            var customers = _customerRepo.GetCustomersForUser(user.userId);
            if (!customers.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User not authorized to view customer");
                
            }
            var json = new JsonResult()
            {
                ContentEncoding = null,
                ContentType = null,
                Data = new { success = true, customer },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return json;
        }

        public ActionResult UpdateCustomer(UpdateCustomerModel request, string userName)
        {
            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var customer = _customerRepo.GetCustomer(request.CustomerId);
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Customer Not Found");
            }

            var user = users.First();
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User not authorized to view customer");
            }

            return _customerRepo.UpdateCustomer(request)
                ? new HttpStatusCodeResult(HttpStatusCode.OK)
                : new HttpStatusCodeResult(500, "A Error occured updating the DB");
        }
    }
}