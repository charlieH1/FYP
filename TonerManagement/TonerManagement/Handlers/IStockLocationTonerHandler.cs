using System.Collections.Generic;
using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers
{
    public interface IStockLocationTonerHandler
    {
        ActionResult TonerOrder(List<TonerOrderModel> order, int stockLocationId, string userName);
    }
}