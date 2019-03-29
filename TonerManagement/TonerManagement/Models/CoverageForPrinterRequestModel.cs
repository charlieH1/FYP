using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class CoverageForPrinterRequestModel
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CoverageType { get; set; }
        public int PrinterId { get; set; }
    }
}