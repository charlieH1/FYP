using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IStockLocationHandler
    {
        
        ActionResult GetStockLocation(int stockLocationId, string userName);
        ActionResult GetStockLocationsForCustomer(int customerId, string userName);
        ActionResult UpdateStockLocation(UpdatedStockLocationModel stockLocationToUpdate, string userName);
        ActionResult GetTonerStockLocationForGrid(int stockLocationId, string userName);
    }
}