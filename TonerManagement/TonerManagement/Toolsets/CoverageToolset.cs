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
        public enum ColorType
        {
            C = 1,
            Y = 2,
            M = 3,
            K = 0
        }
        private readonly ITonerPrinterRepo _tonerPrinterRepo;

        public CoverageToolset(ITonerPrinterRepo tonerPrinterRepo)
        {
            _tonerPrinterRepo = tonerPrinterRepo;
        }

        public double[] GetArrayRangeOfCoverageDaily(DateTime startDate, DateTime endDate,int printerId,ColorType color)
        {
            var tonerPrinterList = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDate, endDate,color);
            var coverageList = new List<double>();
            
            foreach (var tP in tonerPrinterList)
            {
                var tonerMasterYield = tP.tonerExpectedYield;
                var pagesPrinted = tP.totalPagesPrinted;
                var tonerChanges = tP.tonerBottelsChanged;
                var nominalCoverage = tP.nominalCoverage;
                var tonerPercentage = tP.tonerPercentage;

                double coverage = (tonerMasterYield / pagesPrinted) * (tonerChanges + (tonerPercentage / 100)) *
                                  nominalCoverage;
                coverageList.Add(coverage);
            }

            return coverageList.ToArray();
        }

        public double[] GetArrayRangeOfCoverageMonthly(DateTime starDate, DateTime endDate, int printerId)
        {
            //end of month values
        }

        public double CalculateCoverage()
        {

        }
    }
}