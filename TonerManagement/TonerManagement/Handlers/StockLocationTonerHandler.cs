using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class StockLocationTonerHandler : IStockLocationTonerHandler
    {
        private readonly IStockLocationTonerRepo _stockLocationTonerRepo;
        private readonly IUserRepo _userRepo;
        private readonly IStockLocationRepo _stockLocationRepo;
        private readonly ICustomerRepo _customerRepo;
        public StockLocationTonerHandler(IStockLocationTonerRepo stockLocationTonerRepo, IUserRepo userRepo, IStockLocationRepo stockLocationRepo, ICustomerRepo customerRepo)
        {
            _stockLocationTonerRepo = stockLocationTonerRepo;
            _userRepo = userRepo;
            _stockLocationRepo = stockLocationRepo;
            _customerRepo = customerRepo;
        }

        public ActionResult TonerOrder(List<TonerOrderModel> order,int stockLocationId, string userName)
        {
            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User Not found");
            }

            var user = users.First();
            var stockLocation = _stockLocationRepo.GetStockLocation(stockLocationId);
            if (stockLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Stock Location not found");
            }
            var customer = _customerRepo.GetCustomer(stockLocation.customerId);
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized,"This customer can not be accessed by this user.");
            }

            var res = _stockLocationTonerRepo.TonerOrder(order, stockLocationId);
            return res ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(503,"An issue occured creating the order in the database");
        }
    }
}