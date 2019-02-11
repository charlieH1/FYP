using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Toolsets
{
    public class CoverageToolset
    {
        private ITonerPrinterRepo _tonerPrinterRepo;

        public CoverageToolset(ITonerPrinterRepo tonerPrinterRepo)
        {
            _tonerPrinterRepo = tonerPrinterRepo;
        }

        public int[] GetArrayRangeOfCoverageDaily(DateTime startDate, DateTime endDate,int printerId)
        {
            var tonerPrinterList = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDate, endDate);
            foreach()
        }

        public int[] GetArrayRangeOfCoverageMonthly(DateTime starDate, DateTime endDate, int printerId)
        {

        }

        public int CalculateCoverage()
        {

        }
    }
}