using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class TonerStockLocationGridModel
    {
        public int TonerId { get; set; }
        public string TonerName { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }

    }
}