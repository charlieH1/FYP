using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Handlers;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets;
using TonerManagement.Toolsets.Interface;


namespace TonerManagementTests.Handlers
{
    [TestClass()]
    public class PrinterTonerHandlerTests
    {
        

        [TestMethod()]
        public void GetCoverageCyanDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "CyanDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019,02,21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1", customerContactNumber = "123456", customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
                {CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> {customer});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult) sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageYellowDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "YellowDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019, 02, 21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageMagentaDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "MagentaDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019, 02, 21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageKeyingDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "KeyingDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019, 02, 21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageColorDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "ColorDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019, 02, 21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT=> mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageAllDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "AllDaily";
            const int customerId = 1;
            var endDate = new DateTime(2019, 02, 25);
            var startDate = new DateTime(2019, 02, 21);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddDays(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddDays(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddDays(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = endDate}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDailyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageCyanMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "CyanMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageYellowMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "YellowMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageMagentaMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "MagentaMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageKeyingMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "KeyingMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageColorMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "ColorMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageAllMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            const int userId = 1;
            const string coverageType = "AllMonthly";
            const int customerId = 1;
            var endDate = new DateTime(2018, 02, 28);
            var startDate = new DateTime(2019, 09, 01);
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "123456",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            var coverageDateList = new List<CoverageDateModel>()
            {
                new CoverageDateModel() {Coverage = 23.9, Date = startDate},
                new CoverageDateModel() {Coverage = 23.85, Date = startDate.AddMonths(1)},
                new CoverageDateModel() {Coverage = 23.95, Date = startDate.AddMonths(2)},
                new CoverageDateModel() {Coverage = 23.12, Date = startDate.AddMonths(3)},
                new CoverageDateModel() {Coverage = 23.65, Date = startDate.AddMonths(1)}
            };

            var coverageRequest = new CoverageForCompanyRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.M));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthlyForCustomer(customerId, startDate, endDate, CoverageToolset.ColorType.K));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageInvalidCoverageTypeReturnsHttpStatusCode422()
        {
            //setup
            var coverageRequest = new CoverageForCompanyRequestModel()
            {
                CoverageType = "INVALID",
                CustomerId = 1,
                EndDate = new DateTime(2019, 02, 02),
                StartDate = new DateTime(2019, 02, 02)
            };
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (HttpStatusCodeResult) sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod()]
        public void GetCoverageInvalidCustomerIdReturnsHttpStatusCode422()
        {
            var coverageRequest = new CoverageForCompanyRequestModel()
            {
                CoverageType = "CyanDaily",
                CustomerId = 66,
                EndDate = new DateTime(2019,02,25),
                StartDate = new DateTime(2019,02,23)
            };
            const int userId = 1;

            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns((Customer) null);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action
            var res = (HttpStatusCodeResult) sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod()]
        public void GetCoverageCustomerNotAssignedToUserReturnsHttpStatusCodeForbidden()
        {
            //setup
            var coverageRequest = new CoverageForCompanyRequestModel()
            {
                CoverageType = "CyanDaily",
                CustomerId = 66,
                EndDate = new DateTime(2019, 02, 25),
                StartDate = new DateTime(2019, 02, 23)
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456",
                customerID = 66,
                customerName = "Test customer"
            };
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>());

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object);

            //action

            var res = (HttpStatusCodeResult) sut.GetCoverage(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden));
        }

        [TestMethod()]
        public void GetTonerLowsForCompanyPrintersAvailableWithSomeLowDataAndCustomerInAvailiableCustomersForUserOnlyReturnsLowTonerPrinters()
        {
            const int customerId = 1;
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();

            var printers = new List<Printer>
            {
                new Printer
                {
                    printerId = 1,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 25,
                    yellowLowPercentage = 25,
                    magentaLowPercentage = 25,
                    keyingLowPercentage = 25,
                    printerName = "testPrinter",
                    stockLocationId = 1
                },
                new Printer
                {
                    printerId = 2,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 27,
                    yellowLowPercentage = 27,
                    magentaLowPercentage = 27,
                    keyingLowPercentage = 27,
                    printerName = "testPrinter2",
                    stockLocationId = 1
                }
            };

            var tonerPrinter1 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 25,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 28,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var tonerPrinter2 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 30,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 32,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var customer = new Customer()
            {
                customerID = customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123456",
                customerName = "TestCustomer",
            };

            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customerId)).Returns(printers);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter2);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> {customer});
            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object);

            // action
            var res = (JsonResult) sut.GetLowTonerLevelsOfCustomerPrinters(customerId,userId);

            //assert
            var expected = new List<TonerPercentageAndPrinterIdModel>
            {
                new TonerPercentageAndPrinterIdModel
                {
                    Cyan = 25,
                    Yellow = 25,
                    Magenta = 25,
                    Keying = 25,
                    PrinterID = 1
                }
            };
            res.Data.Should().BeEquivalentTo(expected);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void GetTonerLowsForCompanyPrintersAvailableWithSomeLowDataAndCustomerNotAvailableCustomersForUserButIsRealCustomerOnlyReturnsHttpStatusCodeForbidden()
        {
            const int customerId = 1;
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();

            var printers = new List<Printer>
            {
                new Printer
                {
                    printerId = 1,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 25,
                    yellowLowPercentage = 25,
                    magentaLowPercentage = 25,
                    keyingLowPercentage = 25,
                    printerName = "testPrinter",
                    stockLocationId = 1
                },
                new Printer
                {
                    printerId = 2,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 27,
                    yellowLowPercentage = 27,
                    magentaLowPercentage = 27,
                    keyingLowPercentage = 27,
                    printerName = "testPrinter2",
                    stockLocationId = 1
                }
            };

            var tonerPrinter1 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 25,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 28,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var tonerPrinter2 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 30,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 32,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var customer = new Customer()
            {
                customerID = customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123456",
                customerName = "TestCustomer",
            };

            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customerId)).Returns(printers);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter2);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns((Customer) null);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> ());
            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object);

            // action
            var res = (HttpStatusCodeResult)sut.GetLowTonerLevelsOfCustomerPrinters(customerId, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }


        [TestMethod()]
        public void GetTonerLowsForCompanyPrintersAvailableWithSomeLowDataAndInvalidCustomerReturnsHttpStatusCode422()
        {
            const int customerId = 1;
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();

            var printers = new List<Printer>
            {
                new Printer
                {
                    printerId = 1,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 25,
                    yellowLowPercentage = 25,
                    magentaLowPercentage = 25,
                    keyingLowPercentage = 25,
                    printerName = "testPrinter",
                    stockLocationId = 1
                },
                new Printer
                {
                    printerId = 2,
                    customerId = customerId,
                    isColour = true,
                    cyanLowPercentage = 27,
                    yellowLowPercentage = 27,
                    magentaLowPercentage = 27,
                    keyingLowPercentage = 27,
                    printerName = "testPrinter2",
                    stockLocationId = 1
                }
            };

            var tonerPrinter1 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 25,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 28,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var tonerPrinter2 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 30,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 32,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var customer = new Customer()
            {
                customerID = customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123456",
                customerName = "TestCustomer",
            };

            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customerId)).Returns(printers);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.C))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.Y))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.M))
                .Returns(tonerPrinter2);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter2);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object);

            // action
            var res = (JsonResult)sut.GetLowTonerLevelsOfCustomerPrinters(customerId, userId);

            //assert
            var expected = new List<TonerPercentageAndPrinterIdModel>
            {
                new TonerPercentageAndPrinterIdModel
                {
                    Cyan = 25,
                    Yellow = 25,
                    Magenta = 25,
                    Keying = 25,
                    PrinterID = 1
                }
            };
            res.Data.Should().BeEquivalentTo(expected);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);
        }
    }

    

}