using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class TonerPercentageAndPrinterIdModel
    {
        public int PrinterID { get; set; }
        public int Cyan { get; set; }
        public int Yellow { get; set; }
        public int Magenta { get; set; }
        public int Keying { get; set; }

    }
}