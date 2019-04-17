using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class TonerIdTonerPercentageAndPrinterModel
    {
        public int DeviceId { get; set; }
        public double CyanPercentage { get; set; }
        public int CyanId { get; set; }
        public double YellowPercentage { get; set; }
        public int YellowId { get; set; }
        public double MagentaPercentage { get; set; }
        public int MagentaId { get; set; }
        public double KeyingPercentage { get; set; }
        public int KeyingId { get; set; }
        
    }
}