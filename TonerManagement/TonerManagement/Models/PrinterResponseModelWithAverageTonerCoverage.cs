using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class PrinterResponseModelWithAverageTonerCoverage
    {
        public int PrinterId { get; set; }
        public string PrinterName { get; set; }
        public double AverageTonerCoverage { get; set; }
        public int CyanLowTonerPercentage { get; set; }
        public int YellowLowTonerPercentage { get; set; }
        public int MagentaLowTonerPercentage { get; set; }
        public int KeyingLowTonerPercentage { get; set; }

    }
}