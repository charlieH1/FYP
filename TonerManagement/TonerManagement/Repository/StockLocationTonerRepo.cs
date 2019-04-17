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

        public bool TonerOrder(List<TonerOrderModel> order, int stockLocationId)
        {
            foreach (var item in order)
            {
                var itemsToUpdate = _db.StockLocationToners.Where(SLT =>
                    SLT.stockLocationId == stockLocationId && SLT.tonerId == item.TonerId);
                if (!itemsToUpdate.Any())
                {
                    var newItem = _db.StockLocationToners.Create();
                    newItem.stockLocationId = stockLocationId;
                    newItem.tonerId = item.TonerId;
                    newItem.quantity = item.Quantity;
                }
                else
                {
                    var itemToUpdate = itemsToUpdate.First();
                    itemToUpdate.quantity += item.Quantity;
                }
            }

            var res = _db.SaveChanges();
            return res > 0;
        }
    }
}