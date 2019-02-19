using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
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

        public CoverageDateModel[] GetArrayRangeOfCoverageDaily(DateTime startDate, DateTime endDate,int printerId,ColorType color)
        {
            var tonerPrinterList = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDate, endDate,color);
            var coverageList = new List<CoverageDateModel>();
            
            foreach (var tP in tonerPrinterList)
            {
                var tonerMasterYield = tP.tonerExpectedYield;
                var pagesPrinted = tP.totalPagesPrinted;
                var tonerChanges = tP.tonerBottelsChanged;
                var nominalCoverage = tP.nominalCoverage;
                var tonerPercentage = tP.tonerPercentage;
                var date = tP.timestamp;
                var coverage = CalculateCoverage(tonerMasterYield, pagesPrinted, tonerChanges, nominalCoverage,
                    tonerPercentage);
                var coverageDateModel = new CoverageDateModel {Coverage = coverage, Date = date};
                coverageList.Add(coverageDateModel);
            }

            return coverageList.ToArray();
        }

        public CoverageDateModel[] GetArrayRangeOfCoverageMonthly(DateTime startDate, DateTime endDate, int printerId,ColorType color)
        {
            var tonerPrinterList = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDate, endDate, color)
                .OrderBy(tp => tp.timestamp)
                .GroupBy(tp => new { tp.timestamp.Year, tp.timestamp.Month })
                .Select(group => group.LastOrDefault()); ;

            var coverageList = new List<CoverageDateModel>();
            foreach (var tP in tonerPrinterList)
            {
                if (tP == null) continue;
                var tonerMasterYield = tP.tonerExpectedYield;
                var pagesPrinted = tP.totalPagesPrinted;
                var tonerChanges = tP.tonerBottelsChanged;
                var nominalCoverage = tP.nominalCoverage;
                var tonerPercentage = tP.tonerPercentage;
                var date = tP.timestamp;
                var coverage = CalculateCoverage(tonerMasterYield, pagesPrinted, tonerChanges, nominalCoverage,
                    tonerPercentage);
                var coverageDateModel = new CoverageDateModel { Coverage = coverage, Date = date };
                coverageList.Add(coverageDateModel);
            }

            return coverageList.ToArray();
        }

        public double CalculateAverageCoverageForWholeLife(int printerId, ColorType color)
        {
            var tonerPrinter = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, color).OrderBy(tp=> tp.timestamp).LastOrDefault();
            if (tonerPrinter != null)
                return CalculateCoverage(tonerPrinter.tonerExpectedYield, tonerPrinter.totalPagesPrinted,
                    tonerPrinter.tonerBottelsChanged, tonerPrinter.nominalCoverage, tonerPrinter.tonerPercentage);
            else
                return 0.0;
        }

        public double CalculateCoverage(double tonerMasterYield,double pagesPrinted, double tonerChanges, double nominalCoverage, double tonerPercentage)
        {
            return (tonerMasterYield / pagesPrinted) * (tonerChanges + (tonerPercentage / 100)) *
                   nominalCoverage;
        }
    }
}