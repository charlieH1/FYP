using System.Collections.Generic;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface IStockLocationTonerRepo
    {
        List<StockLocationToner> GetStockLocationTonersForStockLocation(int stockLocationId);
        bool TonerOrder(List<TonerOrderModel> order, int stockLocationId);
    }
}