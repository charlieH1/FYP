using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets;

namespace TonerManagementTests.Toolsets
{
    [TestClass()]
    public class CoverageToolsetTests
    {

        
        [TestMethod()]
        public void GetArrayRangeOfCoverageDailyTestWithValidDataReturnsCoverageForData()
        {
            //setup
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var coverageData = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 19),
                    Toner = new Toner(),
                    tonerBottelsChanged = 11,
                    tonerExpectedYield = 56000,
                    tonerId = 1,
                    tonerPercentage = 50,
                    tonerPrinterId = 1
                },
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 18),
                    Toner = new Toner(),
                    tonerBottelsChanged = 13,
                    tonerExpectedYield = 39000,
                    tonerId = 2,
                    tonerPercentage = 50,
                    tonerPrinterId = 2
                }
            };
            var startDate = new DateTime(2019, 02, 17);
            var endDate = new DateTime(2019, 02, 19);
            const int printerId = 1;
            const CoverageToolset.ColorType colorType = CoverageToolset.ColorType.C;
            mockTonerPrinterRepo.Setup(mtp => mtp.GetTonerPrinterForDevice(printerId, startDate,
                endDate, colorType)).Returns(coverageData);
            
            var sut = new CoverageToolset(mockTonerPrinterRepo.Object);

            //action
            var res = sut.GetArrayRangeOfCoverageDaily(startDate, endDate, printerId, colorType);
            
            //assert
            var expectedResultList = new List<CoverageDateModel>
            {
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[0].tonerExpectedYield, coverageData[0].totalPagesPrinted,
                        coverageData[0].tonerBottelsChanged, coverageData[0].nominalCoverage,
                        coverageData[0].tonerPercentage),
                    Date = coverageData[0].timestamp
                },
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[1].tonerExpectedYield, coverageData[1].totalPagesPrinted,
                        coverageData[1].tonerBottelsChanged, coverageData[1].nominalCoverage,
                        coverageData[1].tonerPercentage),
                    Date = coverageData[1].timestamp
                }
            };
            var expectedResultArray = expectedResultList.ToArray();
            res.Should().BeEquivalentTo(expectedResultArray);
            
        }

        [TestMethod()]
        public void GetArrayRangeOfCoverageMonthlyTestWithValidDataReturnsCoverageForMonthsAvailiable()
        {
            //setup
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var coverageData = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 19),
                    Toner = new Toner(),
                    tonerBottelsChanged = 11,
                    tonerExpectedYield = 56000,
                    tonerId = 1,
                    tonerPercentage = 50,
                    tonerPrinterId = 1
                },
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 01, 18),
                    Toner = new Toner(),
                    tonerBottelsChanged = 13,
                    tonerExpectedYield = 39000,
                    tonerId = 2,
                    tonerPercentage = 50,
                    tonerPrinterId = 2
                }
            };
            var startDate = new DateTime(2019, 01, 17);
            var endDate = new DateTime(2019, 02, 19);
            const int printerId = 1;
            const CoverageToolset.ColorType colorType = CoverageToolset.ColorType.C;
            mockTonerPrinterRepo.Setup(mtp => mtp.GetTonerPrinterForDevice(printerId, startDate,
                endDate, colorType)).Returns(coverageData);

            var sut = new CoverageToolset(mockTonerPrinterRepo.Object);

            //action
            var res = sut.GetArrayRangeOfCoverageMonthly(startDate, endDate, printerId, colorType);

            //assert
            var expectedResultList = new List<CoverageDateModel>
            {
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[0].tonerExpectedYield, coverageData[0].totalPagesPrinted,
                        coverageData[0].tonerBottelsChanged, coverageData[0].nominalCoverage,
                        coverageData[0].tonerPercentage),
                    Date = coverageData[0].timestamp
                },
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[1].tonerExpectedYield, coverageData[1].totalPagesPrinted,
                        coverageData[1].tonerBottelsChanged, coverageData[1].nominalCoverage,
                        coverageData[1].tonerPercentage),
                    Date = coverageData[1].timestamp
                }

            };
            var expectedResultArray = expectedResultList.ToArray();
            res.Should().BeEquivalentTo(expectedResultArray);
        }

        [TestMethod()]
        public void CalculateAverageCoverageForWholeLifeTestWithValidDataReturnsLatestCoverageReading()
        {
            //setup
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var coverageData = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 19),
                    Toner = new Toner(),
                    tonerBottelsChanged = 11,
                    tonerExpectedYield = 56000,
                    tonerId = 1,
                    tonerPercentage = 50,
                    tonerPrinterId = 1
                },
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 18),
                    Toner = new Toner(),
                    tonerBottelsChanged = 13,
                    tonerExpectedYield = 39000,
                    tonerId = 2,
                    tonerPercentage = 50,
                    tonerPrinterId = 2
                }
            };
            const int printerId = 1;
            const CoverageToolset.ColorType colorType = CoverageToolset.ColorType.C;
            mockTonerPrinterRepo.Setup(mtp => mtp.GetTonerPrinterForDevice(printerId, colorType)).Returns(coverageData);

            var sut = new CoverageToolset(mockTonerPrinterRepo.Object);

            //action
            var res = sut.CalculateAverageCoverageForWholeLife(printerId, colorType);

            //assert
            var expectedResult = CalculateCoverage(coverageData[0].tonerExpectedYield,
                coverageData[0].totalPagesPrinted,
                coverageData[0].tonerBottelsChanged, coverageData[0].nominalCoverage,
                coverageData[0].tonerPercentage);
            
            
            res.Should().Be(expectedResult);
        }

        private static double CalculateCoverage(double tonerMasterYield, double pagesPrinted, double tonerChanges, double nominalCoverage, double tonerPercentage)
        {
            return (tonerMasterYield / pagesPrinted) * (tonerChanges + (tonerPercentage / 100)) *
                   nominalCoverage;
        }
    }
}