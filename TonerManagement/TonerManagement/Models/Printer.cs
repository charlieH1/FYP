//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TonerManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Printer
    {
        public int printerId { get; set; }
        public int customerId { get; set; }
        public string printerName { get; set; }
        public bool isColour { get; set; }
        public Nullable<int> cyanLowPercentage { get; set; }
        public Nullable<int> yellowLowPercentage { get; set; }
        public Nullable<int> magentaLowPercentage { get; set; }
        public int keyingLowPercentage { get; set; }
        public int stockLocationId { get; set; }
    }
}
