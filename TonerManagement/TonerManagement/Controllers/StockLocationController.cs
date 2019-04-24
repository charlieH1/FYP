using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class StockLocationController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly IStockLocationHandler _stockLocationHandler;
        public StockLocationController(IUserHandler userHandler, IStockLocationHandler stockLocationHandler)
        {
            _userHandler = userHandler;
            _stockLocationHandler = stockLocationHandler;
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


        public ActionResult GetStockLocationsForCustomer(int customerId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _stockLocationHandler.GetStockLocationsForCustomer(customerId, (string) Session["UserName"]);
        }

        public ActionResult GetStockLocation(int stockLocationId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _stockLocationHandler.GetStockLocation(stockLocationId, (string) Session["UserName"]);
        }

        public ActionResult UpdateStockLocation(UpdatedStockLocationModel stockLocationToUpdate)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _stockLocationHandler.UpdateStockLocation(stockLocationToUpdate, (string) Session["UserName"]);
        }

        public ActionResult GetTonerStockLocationForStockLocationForGrid(int stockLocationId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _stockLocationHandler.GetTonerStockLocationForGrid(stockLocationId, (string) Session["UserName"]);
        }
    }
}