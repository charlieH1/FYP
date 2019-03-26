using System.Collections.Generic;
using System.Net;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface IStockLocationRepo
    {
        StockLocation GetStockLocation(int stockLocationId);
        List<StockLocation> GetStockLocationsForCustomer(int customerId);
        bool UpdateStockLocation(StockLocation stockLocation);
    }
}