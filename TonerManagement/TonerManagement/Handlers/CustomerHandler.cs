using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
    }
}