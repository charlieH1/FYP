using System;
using System.Collections.Generic;
using TonerManagement.Models;

namespace TonerManagement.Toolsets.Interface
{
    public interface ICoverageToolset
    {
        double CalculateAverageCoverageForWholeLife(int printerId, CoverageToolset.ColorType color);
        double CalculateCoverage(double tonerMasterYield, double pagesPrinted, double tonerChanges, double nominalCoverage, double tonerPercentage);
        List<CoverageDateModel> GetListOfCoverageDaily(DateTime startDate, DateTime endDate, int printerId, CoverageToolset.ColorType color);
        List<CoverageDateModel> GetListOfCoverageDailyForCustomer(int customerId, DateTime startDate, DateTime endDate, CoverageToolset.ColorType color);
        List<CoverageDateModel> GetListOfCoverageMonthly(DateTime startDate, DateTime endDate, int printerId, CoverageToolset.ColorType color);
        List<CoverageDateModel> GetListOfCoverageMonthlyForCustomer(int customerId, DateTime startDate, DateTime endDate, CoverageToolset.ColorType color);
    }
}