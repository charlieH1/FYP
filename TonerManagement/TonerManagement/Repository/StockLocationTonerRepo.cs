using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class StockLocationTonerRepo : IStockLocationTonerRepo
    {
        private readonly TonerManagementEntities _db;

        public StockLocationTonerRepo(DbContext db)
        {
            _db = (TonerManagementEntities) db;
        }

        public List<StockLocationToner> GetStockLocationTonersForStockLocation(int stockLocationId)
        {
            return _db.StockLocationToners.Where(slt => slt.stockLocationId == stockLocationId).ToList();
        }
    }
}