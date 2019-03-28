using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class StockLocationHandler : IStockLocationHandler
    {
        private readonly IStockLocationRepo _stockLocationRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IUserRepo _userRepo;
        private readonly ITonerRepo _tonerRepo;
        private readonly IStockLocationTonerRepo _stockLocationTonerRepo;
        public StockLocationHandler(IStockLocationRepo stockLocationRepo, ICustomerRepo customerRepo, IUserRepo userRepo, ITonerRepo tonerRepo,IStockLocationTonerRepo stockLocationTonerRepo)
        {
            _stockLocationRepo = stockLocationRepo;
            _customerRepo = customerRepo;
            _userRepo = userRepo;
            _tonerRepo = tonerRepo;
            _stockLocationTonerRepo = stockLocationTonerRepo;
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

            
            var data = new { success = true, StockLocation = stockLocation };

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

        public ActionResult GetTonerStockLocationForGrid(int stockLocationId, string userName)
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

            var gridData = new List<TonerStockLocationGridModel>();

            var stockLocationToners = _stockLocationTonerRepo.GetStockLocationTonersForStockLocation(stockLocationId);
            foreach (var stockLocationToner in stockLocationToners)
            {
                var gridDataElement = new TonerStockLocationGridModel {TonerId = stockLocationToner.tonerId};
                var tonerItem = _tonerRepo.GetToner(stockLocationToner.tonerId);
                if (tonerItem == null) continue;
                if (tonerItem.isCyan)
                {
                    gridDataElement.Color = "Cyan";
                }
                else if (tonerItem.isKeying)
                {
                    gridDataElement.Color = "Keying";
                }
                else if (tonerItem.isMagenta)
                {
                    gridDataElement.Color = "Magenta";
                }
                else if (tonerItem.isYellow)
                {
                    gridDataElement.Color = "Yellow";
                }

                gridDataElement.Quantity = stockLocationToner.quantity;
                gridDataElement.TonerName = tonerItem.tonerName;
                gridData.Add(gridDataElement);
            }
            return new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data=gridData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}