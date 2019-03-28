using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class CoverageGridModel
    {
        public int PrinterId { get; set; }
        public double CurrentCoverage { get; set; }
        public double MonthCoverage { get; set; }
        public double SixMonthCoverage { get; set; }
        public double YearCoverage { get; set; }
    }
}