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
    public class TonerOrderController : Controller
    {
        private readonly IDevicesHandler _devicesHandler;
        private readonly IUserHandler  _userHandler;
        private readonly IStockLocationTonerHandler _stockLocationTonerHandler;
        public TonerOrderController(IDevicesHandler devicesHandler, IUserHandler userHandler, IStockLocationTonerHandler stockLocationTonerHandler)
        {
            _userHandler = userHandler;
            _devicesHandler = devicesHandler;
            _stockLocationTonerHandler = stockLocationTonerHandler;
        }
        // GET: TonerOrder
        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View("Index");
        }

        public ActionResult GetTonerPercentageAndIdsForPrintersPerStockLocation(int stockLocationId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return _devicesHandler.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId, (string)Session["UserName"]);

        }

        public ActionResult CreateOrder(List<int> tonerIds, List<int> tonerQuantities, int stockLocationId)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (tonerIds.Count != tonerQuantities.Count)
            {
                return new HttpStatusCodeResult(422, "Toner id's must be equal in length to the quantities");
            }

            var tonerOrder = tonerIds.Select((t, i) => new TonerOrderModel {TonerId = t, Quantity = tonerQuantities[i]}).ToList();

            return _stockLocationTonerHandler.TonerOrder(tonerOrder, stockLocationId, (string) Session["UserName"]);
        }
    }
}