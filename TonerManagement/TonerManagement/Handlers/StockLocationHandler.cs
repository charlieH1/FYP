using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class StockLocationHandler : IStockLocationHandler
    {
        private readonly IStockLocationRepo _stockLocationRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IUserRepo _userRepo;
        public StockLocationHandler(IStockLocationRepo stockLocationRepo, ICustomerRepo customerRepo, IUserRepo userRepo)
        {
            _stockLocationRepo = stockLocationRepo;
            _customerRepo = customerRepo;
            _userRepo = userRepo;
        }

        

        public ActionResult GetStockLocationsForCustomer(int customerId, string userName)
        {
            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized,"User not known");
            }

            var user = users.First();
            var customer = _customerRepo.GetCustomer(customerId);
            var customers = _customerRepo.GetCustomersForUser(user.userId);
            if (!customers.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }

            var stockLocations = _stockLocationRepo.GetStockLocationsForCustomer(customerId);
            

            var json = new JsonResult()
            {
                Data = stockLocations,
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return json;
            
        }

        public ActionResult GetStockLocation(int stockLocationId, string userName)
        {
            var users = _userRepo.GetUsers(userName);
            var stockLocation = _stockLocationRepo.GetStockLocation(stockLocationId);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known");
            }

            if (stockLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "The stock location could not be found");
            }
            var user = users.First();
            var customer = _customerRepo.GetCustomer(stockLocation.customerId);
            var customers = _customerRepo.GetCustomersForUser(user.userId);
            if (!customers.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }

            
            var data = new { sucesss = true, StockLocation = stockLocation };

            var json = new JsonResult()
            {
                Data = data,
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return json;
        }

        public ActionResult UpdateStockLocation(UpdatedStockLocationModel stockLocationToUpdate, string userName)
        {

            var users = _userRepo.GetUsers(userName);
            var stockLocation = _stockLocationRepo.GetStockLocation(stockLocationToUpdate.StockLocationId);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known");
            }

            if (stockLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "The stock location could not be found");
            }
            var user = users.First();
            var customer = _customerRepo.GetCustomer(stockLocation.customerId);
            var customers = _customerRepo.GetCustomersForUser(user.userId);
            if (!customers.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }

            var phoneNumberRegEx = new Regex(
                @"((?: \+| 00)[17](?: |\-) ?| (?: \+| 00)[1 - 9]\d{ 0, 2 } (?: |\-)?| (?: \+| 00) 1\-\d{ 3 } (?: |\-)?)?(0\d |\([0 - 9]{ 3 } \)| [1 - 9]{ 0, 3 }) (?: ((?: |\-)[0 - 9]{ 2 }) { 4 }| ((?: [0 - 9]{ 2 }) { 4 })| ((?: |\-)[0 - 9]{ 3 } (?: |\-)[0 - 9]{ 4 })| ([0 - 9]{ 7 }))");

            if (stockLocationToUpdate.StockLocationAddress.IsNullOrWhiteSpace())
            {
                return new HttpStatusCodeResult(422,"Stock Location Address cant be null or whitespace");
            }

            if (stockLocationToUpdate.StockLocationName.IsNullOrWhiteSpace())
            {
                return new HttpStatusCodeResult(422,"Stock Location Name cant be null or whitespace");
            }

            if (stockLocationToUpdate.StockLocationContactNumber.IsNullOrWhiteSpace())
            {
                return new HttpStatusCodeResult(422, "Stock Location phone number cant be null or whitespace");
            }

            if (!phoneNumberRegEx.IsMatch(stockLocationToUpdate.StockLocationContactNumber))
            {
                return new HttpStatusCodeResult(422, "Phone number is not valid");
            }

            

            var stockLocationValidated = new StockLocation
            {
                stockLocationId = stockLocationToUpdate.StockLocationId,
                stockLocationAddress = stockLocationToUpdate.StockLocationAddress,
                stockLocationContactNumber = stockLocationToUpdate.StockLocationContactNumber,
                stockLocationName = stockLocationToUpdate.StockLocationName
            };

            var success = _stockLocationRepo.UpdateStockLocation(stockLocationValidated);
            return success == true ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(422, "Failed to update DB");

        }
    }
}