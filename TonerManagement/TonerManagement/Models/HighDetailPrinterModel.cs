using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class HighDetailPrinterModel
    {
        public int PrinterId { get; set; }
        public string PrinterName { get; set; }
        public int CyanLevel { get; set; }
        public int YellowLevel { get; set; }
        public int MagentaLevel { get; set; }
        public int KeyingLevel { get; set; }
        public double CyanCoverage { get; set; }
        public double YellowCoverage { get; set; }
        public double MagentaCoverage { get; set; }
        public double KeyingCoverage { get; set; }
        public double AverageCoverage { get; set; }
    }
}