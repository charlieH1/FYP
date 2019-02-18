using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
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
                double tonerMasterYield = tP.tonerExpectedYield;
                double pagesPrinted = tP.totalPagesPrinted;
                double tonerChanges = tP.tonerBottelsChanged;
                double nominalCoverage = tP.nominalCoverage;
                double tonerPercentage = tP.tonerPercentage;

                var coverage = CalculateCoverage(tonerMasterYield, pagesPrinted, tonerChanges, nominalCoverage,
                    tonerPercentage);
                coverageList.Add(coverage);
            }

            return coverageList.ToArray();
        }

        public double[] GetArrayRangeOfCoverageMonthly(DateTime startDate, DateTime endDate, int printerId,ColorType color)
        {
            var tonerPrinterList = from tp in _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDate, endDate, color) group tp by new{tp.timestamp.Year, tp.timestamp.Month} into tpg orderby tpg.Key select tpg.OrderByDescending(tpgx=> tpgx.timestamp).FirstOrDefault();

            return (from tP in tonerPrinterList let tonerMasterYield = tP.tonerExpectedYield let pagesPrinted = tP.totalPagesPrinted let tonerChanges = tP.tonerBottelsChanged let nominalCoverage = tP.nominalCoverage let tonerPercentage = tP.tonerPercentage select CalculateCoverage(tonerMasterYield, pagesPrinted, tonerChanges, nominalCoverage, tonerPercentage)).ToArray();
        }

        public double CalculateCoverage(double tonerMasterYield,double pagesPrinted, double tonerChanges, double nominalCoverage, double tonerPercentage)
        {
            return (tonerMasterYield / pagesPrinted) * (tonerChanges + (tonerPercentage / 100)) *
                   nominalCoverage;
        }
    }
}