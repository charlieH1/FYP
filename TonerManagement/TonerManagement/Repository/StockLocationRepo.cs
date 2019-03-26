using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class StockLocationRepo : IStockLocationRepo
    {
        private readonly TonerManagementEntities _db;
        public StockLocationRepo(DbContext db)
        {
            _db = (TonerManagementEntities) db;
        }

        public List<StockLocation> GetStockLocationsForCustomer(int customerId)
        {
            return _db.StockLocations.Where(SL => SL.customerId == customerId).ToList();
        }

        public StockLocation GetStockLocation(int stockLocationId)
        {
            return _db.StockLocations.Find(stockLocationId);
        }

        public bool UpdateStockLocation(StockLocation stockLocation)
        {
            var oldStockLocation =
                _db.StockLocations.Single(
                    sl => sl.stockLocationId == stockLocation.stockLocationId);
            oldStockLocation.stockLocationAddress = stockLocation.stockLocationAddress;
            oldStockLocation.stockLocationName = stockLocation.stockLocationName;
            oldStockLocation.stockLocationContactNumber = stockLocation.stockLocationContactNumber;
            var updated = _db.SaveChanges();
            return updated > 0;
        }

    }
}