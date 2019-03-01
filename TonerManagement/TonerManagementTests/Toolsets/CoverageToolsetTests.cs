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
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            
            var sut = new CoverageToolset(mockTonerPrinterRepo.Object,mockPrinterRepo.Object);

            //action
            var res = sut.GetListOfCoverageDaily(startDate, endDate, printerId, colorType);
            
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
            res.Should().BeEquivalentTo(expectedResultList);
            
        }

        [TestMethod()]
        public void GetArrayRangeOfCoverageMonthlyTestWithValidDataReturnsCoverageForMonthsAvailable()
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
            mockTonerPrinterRepo.Setup(mtp => mtp.GetTonerPrinterForDevice(printerId, new DateTime(startDate.Year,startDate.Month,1),
                new DateTime(endDate.Year,endDate.Month,DateTime.DaysInMonth(endDate.Year,endDate.Month)), colorType)).Returns(coverageData);
            var mockPrinterRepo = new Mock<IPrinterRepo>();


            var sut = new CoverageToolset(mockTonerPrinterRepo.Object,mockPrinterRepo.Object);

            //action
            var res = sut.GetListOfCoverageMonthly(startDate, endDate, printerId, colorType);

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
            res.Should().BeEquivalentTo(expectedResultList);
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
            var mockPrinterRepo = new Mock<IPrinterRepo>();

            var sut = new CoverageToolset(mockTonerPrinterRepo.Object,mockPrinterRepo.Object);

            //action
            var res = sut.CalculateAverageCoverageForWholeLife(printerId, colorType);

            //assert
            var expectedResult = CalculateCoverage(coverageData[0].tonerExpectedYield,
                coverageData[0].totalPagesPrinted,
                coverageData[0].tonerBottelsChanged, coverageData[0].nominalCoverage,
                coverageData[0].tonerPercentage);
            
            
            res.Should().Be(expectedResult);
        }

        [TestMethod]
        public void GetListOfCoverageDailyForCompanyReturnsCoverage()
        {
            var startDate = new DateTime(2019,02,17);
            var endDate = new DateTime(2019,02,19);
            const CoverageToolset.ColorType color = CoverageToolset.ColorType.C;
            const int customerId = 1;
            var printerData = new List<Printer>()
            {
                new Printer
                {
                    printerId = 1,
                    customerId = 1
                },
                new Printer
                {
                    printerId = 2,
                    customerId = 1
                }
            };
            var coverageDataP1 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 17),
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

            var coverageDataP2 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 2,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 18),
                    Toner = new Toner(),
                    tonerBottelsChanged = 11,
                    tonerExpectedYield = 56000,
                    tonerId = 1,
                    tonerPercentage = 50,
                    tonerPrinterId = 1
                },
                new TonerPrinter
                {
                    printerId = 2,
                    nominalCoverage = 8.5,
                    Printer = new Printer(),
                    timestamp = new DateTime(2019, 02, 19),
                    Toner = new Toner(),
                    tonerBottelsChanged = 13,
                    tonerExpectedYield = 39000,
                    tonerId = 2,
                    tonerPercentage = 50,
                    tonerPrinterId = 2
                }
            };

            var mockPrinterRepo = new Mock<IPrinterRepo>();
            mockPrinterRepo.Setup(pr => pr.GetPrintersFromCustomer(customerId)).Returns(printerData);

            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            mockTonerPrinterRepo.Setup(tp => tp.GetTonerPrinterForDevice(1, startDate, startDate, color))
                .Returns(new List<TonerPrinter> {coverageDataP1[0]});
            mockTonerPrinterRepo.Setup(tp => tp.GetTonerPrinterForDevice(2, startDate, startDate, color))
                .Returns(new List<TonerPrinter>());
            mockTonerPrinterRepo
                .Setup(tp => tp.GetTonerPrinterForDevice(1, startDate.AddDays(1), startDate.AddDays(1), color))
                .Returns(new List<TonerPrinter> {coverageDataP1[1]});
            mockTonerPrinterRepo
                .Setup(tp => tp.GetTonerPrinterForDevice(2, startDate.AddDays(1), startDate.AddDays(1), color))
                .Returns(new List<TonerPrinter> {coverageDataP2[0]});
            mockTonerPrinterRepo.Setup(tp => tp.GetTonerPrinterForDevice(1, endDate, endDate, color))
                .Returns(new List<TonerPrinter>());
            mockTonerPrinterRepo.Setup(tp => tp.GetTonerPrinterForDevice(2, endDate, endDate, color))
                .Returns(new List<TonerPrinter> {coverageDataP2[1]});

            var sut = new CoverageToolset(mockTonerPrinterRepo.Object,mockPrinterRepo.Object);

            //action

            var res = sut.GetListOfCoverageDailyForCustomer(1, startDate, endDate, color);

            //assert
            var expected = new List<CoverageDateModel>
            {
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageDataP1[0].tonerExpectedYield,
                        coverageDataP1[0].totalPagesPrinted, coverageDataP1[0].tonerBottelsChanged,
                        coverageDataP1[0].nominalCoverage, coverageDataP1[0].tonerPercentage),
                    Date = coverageDataP1[0].timestamp
                },
                new CoverageDateModel
                {
                    Coverage = (CalculateCoverage(coverageDataP1[1].tonerExpectedYield,
                                    coverageDataP1[1].totalPagesPrinted, coverageDataP1[1].tonerBottelsChanged,
                                    coverageDataP1[1].nominalCoverage, coverageDataP1[1].tonerPercentage) +
                                CalculateCoverage(coverageDataP2[0].tonerExpectedYield,
                                    coverageDataP2[0].totalPagesPrinted, coverageDataP2[0].tonerBottelsChanged,
                                    coverageDataP2[0].nominalCoverage, coverageDataP2[0].tonerPercentage)) / 2,
                    Date = coverageDataP1[1].timestamp

                },
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageDataP2[1].tonerExpectedYield,
                        coverageDataP2[1].totalPagesPrinted, coverageDataP2[1].tonerBottelsChanged,
                        coverageDataP2[1].nominalCoverage, coverageDataP2[1].tonerPercentage),
                    Date = coverageDataP2[1].timestamp
                }
            };
            res.Should().BeEquivalentTo(expected);

        }

        [TestMethod]
        public void GetListOfCoverageMonthlyForCompanyWithValidDateReturnsCoverage()
        {
            //setup
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var printerData = new List<Printer>
            {
                new Printer
                {
                    printerId = 1,
                    customerId = 1,
                }
            };
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
            const int customerId = 1;
            const CoverageToolset.ColorType colorType = CoverageToolset.ColorType.C;
            mockTonerPrinterRepo.Setup(mtp => mtp.GetTonerPrinterForDevice(printerData[0].printerId, new DateTime(2019,1,1),
               new DateTime(2019,1,31), colorType)).Returns(new List<TonerPrinter>{coverageData[0]});
            mockTonerPrinterRepo
                .Setup(mtp => mtp.GetTonerPrinterForDevice(printerData[0].printerId, new DateTime(2019, 2, 1),
                    new DateTime(2019, 2, 28), colorType)).Returns(new List<TonerPrinter> {coverageData[1]});
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            mockPrinterRepo.Setup(pr => pr.GetPrintersFromCustomer(customerId)).Returns(printerData);


            var sut = new CoverageToolset(mockTonerPrinterRepo.Object, mockPrinterRepo.Object);

            //action
            var res = sut.GetListOfCoverageMonthlyForCustomer(customerId,startDate, endDate, colorType);

            //assert
            var expectedResultList = new List<CoverageDateModel>
            {
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[1].tonerExpectedYield, coverageData[1].totalPagesPrinted,
                        coverageData[1].tonerBottelsChanged, coverageData[1].nominalCoverage,
                        coverageData[1].tonerPercentage),
                    Date = new DateTime(2019,02,19)
                },
                new CoverageDateModel
                {
                    Coverage = CalculateCoverage(coverageData[0].tonerExpectedYield, coverageData[0].totalPagesPrinted,
                        coverageData[0].tonerBottelsChanged, coverageData[0].nominalCoverage,
                        coverageData[0].tonerPercentage),
                    Date = new DateTime(2019,01,18)
                }

            };
            res.Should().BeEquivalentTo(expectedResultList);
        }

        private static double CalculateCoverage(double tonerMasterYield, double pagesPrinted, double tonerChanges, double nominalCoverage, double tonerPercentage)
        {
            return (tonerMasterYield / pagesPrinted) * (tonerChanges + (tonerPercentage / 100)) *
                   nominalCoverage;
        }
    }
}