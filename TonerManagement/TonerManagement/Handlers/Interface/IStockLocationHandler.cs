using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers
{
    public interface IStockLocationHandler
    {
        
        ActionResult GetStockLocation(int stockLocationId, string userName);
        ActionResult GetStockLocationsForCustomer(int customerId, string userName);
        ActionResult UpdateStockLocation(UpdatedStockLocationModel stockLocationToUpdate, string userName);
    }
}