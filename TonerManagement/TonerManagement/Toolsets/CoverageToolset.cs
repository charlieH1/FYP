using System;
using System.Collections.Generic;
using System.Linq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets.Interface;

namespace TonerManagement.Toolsets
{
    public class CoverageToolset : ICoverageToolset
    {
        public enum ColorType
        {
            C = 1,
            Y = 2,
            M = 3,
            K = 0
        }
        private readonly ITonerPrinterRepo _tonerPrinterRepo;
        private readonly IPrinterRepo _printerRepo;

        public CoverageToolset(ITonerPrinterRepo tonerPrinterRepo, IPrinterRepo printerRepo)
        {
            _tonerPrinterRepo = tonerPrinterRepo;
            _printerRepo = printerRepo;
        }

        /// <summary>
        /// Gets daily coverage for a printer
        /// </summary>
        /// <param name="startDate"> the start date</param>
        /// <param name="endDate"> the end date</param>
        /// <param name="printerId"> the prineter id</param>
        /// <param name="color"> the color</param>
        /// <returns> a list of type CoverageDateModel that contains the coverage details</returns>
        public List<CoverageDateModel> GetListOfCoverageDaily(DateTime startDate, DateTime endDate,int printerId,ColorType color)
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

            return coverageList;
        }

        /// <summary>
        /// Gets the monthly coverage for a device 
        /// </summary>
        /// <param name="startDate"> the start date</param>
        /// <param name="endDate"> the end date</param>
        /// <param name="printerId"> the printer id</param>
        /// <param name="color"> the color</param>
        /// <returns> the monthly coverage for a device</returns>
        public List<CoverageDateModel> GetListOfCoverageMonthly(DateTime startDate, DateTime endDate, int printerId,ColorType color)
        {
            var startDateForMonth = new DateTime(startDate.Year,startDate.Month,1);
            var endDateForMonth = new DateTime(endDate.Year,endDate.Month,DateTime.DaysInMonth(endDate.Year,endDate.Month));
            var tonerPrinterList = _tonerPrinterRepo.GetTonerPrinterForDevice(printerId, startDateForMonth, endDateForMonth, color);
            if (tonerPrinterList == null)
            {
                return new List<CoverageDateModel>();
            }
            var tonerPrinterListResult = tonerPrinterList
                .OrderBy(tp => tp.timestamp)
                .GroupBy(tp => new { tp.timestamp.Year, tp.timestamp.Month })
                .Select(group => group.LastOrDefault()); 

            var coverageList = new List<CoverageDateModel>();
            foreach (var tP in tonerPrinterListResult)
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

            return coverageList;
        }

        /// <summary>
        /// Gets the monthly coverage for a customer
        /// </summary>
        /// <param name="customerId"> the customer id</param>
        /// <param name="startDate"> the start date</param>
        /// <param name="endDate"> the end date</param>
        /// <param name="color"> the color</param>
        /// <returns>the monthly coverage for a customer</returns>
        public List<CoverageDateModel> GetListOfCoverageMonthlyForCustomer(int customerId, DateTime startDate, DateTime endDate,ColorType color)
        {
            //fetch all printer ids for a company
            var printers =_printerRepo.GetPrintersFromCustomer(customerId);
            //get coverage for each month for all printers
            var startDateCounter = startDate;
            var averageCoverageForMonths = new List<CoverageDateModel>();
            
            while (startDateCounter < endDate)
            {
                var coverageForMonth = new List<CoverageDateModel>();
                foreach (var printer in printers)
                {
                    //get coverage for each printer
                    var tp = GetListOfCoverageMonthly(startDateCounter, new DateTime(startDateCounter.Year,
                        startDateCounter.Month,
                        DateTime.DaysInMonth(startDateCounter.Year, startDateCounter.Month)), printer.printerId, color);
                    if (tp.Count > 0)
                    {
                        coverageForMonth.Add(tp.First());
                    }
                    
                }

                
                var totalDevices = coverageForMonth.Count;
                CoverageDateModel averageCoverageForMonth;
                if (totalDevices == 0)
                {
                    averageCoverageForMonth = new CoverageDateModel
                    {
                        Coverage = 0,
                        Date = startDateCounter
                    };
                }
                else
                {
                    var totalCoverage = coverageForMonth.Sum(tp => tp.Coverage);
                    averageCoverageForMonth = new CoverageDateModel
                        {Coverage = totalCoverage / totalDevices, Date = coverageForMonth.Max(tp => tp.Date)};
                }

                averageCoverageForMonths.Add(averageCoverageForMonth);
                startDateCounter = startDateCounter.AddMonths(1);
            }

            return averageCoverageForMonths;


        }

        /// <summary>
        /// Gets the daily coverage for a customer
        /// </summary>
        /// <param name="customerId"> the customer id</param>
        /// <param name="startDate"> the start date</param>
        /// <param name="endDate"> the end date</param>
        /// <param name="color"> the color</param>
        /// <returns>the daily coverage for a customer</returns>
        public List<CoverageDateModel> GetListOfCoverageDailyForCustomer(int customerId, DateTime startDate, DateTime endDate, ColorType color)
        {
            //fetch all printer ids for a company
            var printers = _printerRepo.GetPrintersFromCustomer(customerId);
            //get coverage for each month for all printers
            var startDateCounter = startDate;
            var averageCoverageForDays = new List<CoverageDateModel>();
            
            while (startDateCounter <= endDate)
            {
                var coverageForDay = new List<CoverageDateModel>();
                foreach (var printer in printers)
                {
                    //get coverage for each printer
                    var tp = GetListOfCoverageDaily(startDateCounter, startDateCounter, printer.printerId, color);
                    if (tp.Count>0)
                    {
                        coverageForDay.Add(tp.First());
                    }
                    
                }

                var totalDevices = coverageForDay.Count;
                CoverageDateModel averageCoverageForDay;
                if (totalDevices == 0)
                {
                    averageCoverageForDay = new CoverageDateModel
                    {
                        Coverage = 0,
                        Date = startDateCounter
                    };
                }
                else
                {
                    var totalCoverage = coverageForDay.Sum(tp => tp.Coverage);
                    averageCoverageForDay = new CoverageDateModel { Coverage = totalCoverage / totalDevices, Date = coverageForDay.Max(tp => tp.Date) };
                    
                }
                averageCoverageForDays.Add(averageCoverageForDay);
                startDateCounter = startDateCounter.AddDays(1);
            }

            return averageCoverageForDays;


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