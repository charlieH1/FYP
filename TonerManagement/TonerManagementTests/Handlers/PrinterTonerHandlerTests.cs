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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult) sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();
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


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverage(coverageRequest, userId);

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
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

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
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns((Customer) null);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

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
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>());

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object,mockCustomerRepo.Object,mockPrinterRepo.Object,mockTonerPrinterRepo.Object,mockUserRepo.Object);

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
            var mockUserRepo = new Mock<IUserRepo>();

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
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            // action
            var res = (JsonNetResult) sut.GetLowTonerLevelsOfCustomerPrinters(customerId,userId);

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
            var mockUserRepo = new Mock<IUserRepo>();

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
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

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
            var mockUserRepo = new Mock<IUserRepo>();

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
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            // action
            var res = (JsonNetResult)sut.GetLowTonerLevelsOfCustomerPrinters(customerId, userId);

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
        public void GetCoverageForPrinterCyanDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            const int userId = 1;
            const string coverageType = "CyanDaily";
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate,PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterYellowDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate, coverageRequest.PrinterId,CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterMagentaDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate,PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterKeyingDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate, coverageRequest.PrinterId,CoverageToolset.ColorType.K))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterColorDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId=1 };

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageForPrinterAllDailyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate,PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily(startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId,CoverageToolset.ColorType.K))
                .Returns(coverageDateList);
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(1)).Returns(new Printer {printerId = 1, isColour = true});

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageDaily(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageForPrinterCyanMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate,PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterYellowMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterMagentaMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate, coverageRequest.PrinterId,CoverageToolset.ColorType.M))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterKeyingMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate,PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly(startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
        }

        [TestMethod()]
        public void GetCoverageForPrinterColorMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly(startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly(startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageForPrinterAllMonthlyValidRequestAndUserReturnsCoverage()
        {
            //setup
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
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

            var coverageRequest = new CoverageForPrinterRequestModel()
            { CoverageType = coverageType, CustomerId = customerId, EndDate = endDate, StartDate = startDate, PrinterId = 1};

            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer> { customer });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate, coverageRequest.PrinterId,CoverageToolset.ColorType.Y))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M))
                .Returns(coverageDateList);
            mockCoverageToolset
                .Setup(mCT =>
                    mCT.GetListOfCoverageMonthly( startDate, endDate,coverageRequest.PrinterId, CoverageToolset.ColorType.K))
                .Returns(coverageDateList);


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Data.Should().BeEquivalentTo(coverageDateList);
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M));
            mockCoverageToolset.Verify(mCT => mCT.GetListOfCoverageMonthly(startDate, endDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCoverageForPrinterInvalidCoverageTypeReturnsHttpStatusCode422()
        {
            //setup
            var coverageRequest = new CoverageForPrinterRequestModel()
            {
                CoverageType = "INVALID",
                CustomerId = 1,
                EndDate = new DateTime(2019, 02, 02),
                StartDate = new DateTime(2019, 02, 02),
                PrinterId = 1
                
            };
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod()]
        public void GetCoverageForPrinterInvalidCustomerIdReturnsHttpStatusCode422()
        {
            var coverageRequest = new CoverageForPrinterRequestModel()
            {
                CoverageType = "CyanDaily",
                CustomerId = 66,
                EndDate = new DateTime(2019, 02, 25),
                StartDate = new DateTime(2019, 02, 23),
                PrinterId = 1
            };
            const int userId = 1;

            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns((Customer)null);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod()]
        public void GetCoverageForPrinterCustomerNotAssignedToUserReturnsHttpStatusCodeForbidden()
        {
            //setup
            var coverageRequest = new CoverageForPrinterRequestModel()
            {
                CoverageType = "CyanDaily",
                CustomerId = 66,
                EndDate = new DateTime(2019, 02, 25),
                StartDate = new DateTime(2019, 02, 23),
                PrinterId = 1
                
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
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(coverageRequest.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>());


            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.GetCoverageForPrinter(coverageRequest, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden));
        }

        [TestMethod()]
        public void GetCoverageGridForPrinterInvalidCustomerIdReturnsHttpStatusCode422()
        {
            const int customerId = 66;
            const int userId = 1;

            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns((Customer)null);

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverageGridForCustomer(customerId, userId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod()]
        public void GetCoverageGridCustomerNotAssignedToUserReturnsHttpStatusCodeForbidden()
        {
            //setup
            
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
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>());

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.GetCoverageGridForCustomer( userId,customer.customerID);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden));
        }

        [TestMethod()]
        public void GetCoverageColorGridValidUserAndRequestUserReturnsCoverage()
        {
            //setup
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456",
                customerID = 66,
                customerName = "Test customer"
            };
            var printer = new Printer()
            {
                printerId = 1,
                customerId = customer.customerID,
                cyanLowPercentage = 25,
                isColour = true,
                keyingLowPercentage = 25,
                magentaLowPercentage = 25,
                printerName = "Test Printer",
                stockLocationId = 23,
                yellowLowPercentage = 25,
            };
            var printerList = new List<Printer>()
            {
                printer
            };
            var coverage = new CoverageDateModel()
            {
                Date = DateTime.Today,
                Coverage = 28.0d
            };
            var coverageGridModel = new CoverageGridModel()
            {
                CurrentCoverage = 28.0d,
                MonthCoverage = 28.0d,
                SixMonthCoverage = 28.0d,
                YearCoverage = 28.0d,
                PrinterId = printer.printerId
            };
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>(){customer});
            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customer.customerID)).Returns(printerList);
            mockCoverageToolset.Setup(mCT =>
                    mCT.CalculateAverageCoverageForWholeLife(printer.printerId, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(28.0d);
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printer.printerId)).Returns(printer);
            mockCoverageToolset.Setup(mCT => mCT.GetListOfCoverageMonthly(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                printer.printerId, It.IsAny<CoverageToolset.ColorType>())).Returns(new List<CoverageDateModel>{coverage});

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action

            var res = (JsonNetResult)sut.GetCoverageGridForCustomer(userId, customer.customerID);

            //assert
            res.Data.Should().BeEquivalentTo(new List<CoverageGridModel>{coverageGridModel});
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void GetCoverageBAndWGridValidUserAndRequestUserReturnsCoverage()
        {
            //setup
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456",
                customerID = 66,
                customerName = "Test customer"
            };
            var printer = new Printer()
            {
                printerId = 1,
                customerId = customer.customerID,
                cyanLowPercentage = 25,
                isColour = false,
                keyingLowPercentage = 25,
                magentaLowPercentage = 25,
                printerName = "Test Printer",
                stockLocationId = 23,
                yellowLowPercentage = 25,
            };
            var printerList = new List<Printer>()
            {
                printer
            };
            var coverage = new CoverageDateModel()
            {
                Date = DateTime.Today,
                Coverage = 28.0d
            };
            var coverageGridModel = new CoverageGridModel()
            {
                CurrentCoverage = 28.0d,
                MonthCoverage = 28.0d,
                SixMonthCoverage = 28.0d,
                YearCoverage = 28.0d,
                PrinterId = printer.printerId
            };
            const int userId = 1;
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>() { customer });
            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customer.customerID)).Returns(printerList);
            mockCoverageToolset.Setup(mCT =>
                    mCT.CalculateAverageCoverageForWholeLife(printer.printerId, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(28.0d);
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printer.printerId)).Returns(printer);
            mockCoverageToolset.Setup(mCT => mCT.GetListOfCoverageMonthly(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                printer.printerId, CoverageToolset.ColorType.K)).Returns(new List<CoverageDateModel> { coverage });

            var sut = new PrinterTonerHandler(mockCoverageToolset.Object, mockCustomerRepo.Object, mockPrinterRepo.Object, mockTonerPrinterRepo.Object,mockUserRepo.Object);

            //action

            var res = (JsonNetResult)sut.GetCoverageGridForCustomer(userId, customer.customerID);

            //assert
            res.Data.Should().BeEquivalentTo(new List<CoverageGridModel> { coverageGridModel });
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void GetCurrentTonerLevelsPrinterNotFoundReturnsHttpStatusCodeNotFound()
        {
            //setup
            var mockCoverageToolSet = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";

            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns((Printer) null);

            var sut = new PrinterTonerHandler(mockCoverageToolSet.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult) sut.GetCurrentTonerLevel(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, "Printer couldn't be found"));

        }

        [TestMethod()]
        public void GetCurrentTonerLevelsUserNotFoundReturnsHttpStatusCodeNotFound()
        {
            //setup
            var mockCoverageToolSet = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";

            var printer = new Printer()
            {
                printerId = printerId,
                printerName = "Test Printer",
                customerId = 1,
                isColour = true,
                cyanLowPercentage = 25,
                yellowLowPercentage = 25,
                magentaLowPercentage = 25,
                keyingLowPercentage = 25,
                stockLocationId = 1,
            };

            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());

            var sut = new PrinterTonerHandler(mockCoverageToolSet.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCurrentTonerLevel(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, userName + " User not found"));

        }


        [TestMethod()]
        public void GetCurrentTonerLevelsUserDoesntHaveAccessReturnsHttpStatusCodeForbiden()
        {
            //setup
            var mockCoverageToolSet = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";

            var printer = new Printer()
            {
                printerId = printerId,
                printerName = "Test Printer",
                customerId = 1,
                isColour = true,
                cyanLowPercentage = 25,
                yellowLowPercentage = 25,
                magentaLowPercentage = 25,
                keyingLowPercentage = 25,
                stockLocationId = 1,
            };

            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd1!"),
                userId = 1,
                userLogin = userName
            };

            var customer = new Customer()
            {
                customerAddress = "Test Customer Address",
                customerContactNumber = "07700900000",
                customerID = printer.customerId,
                customerName = "Test Customer Name",
            };

            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>(){user});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(printer.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new PrinterTonerHandler(mockCoverageToolSet.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCurrentTonerLevel(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));

        }

        [TestMethod()]
        public void GetCurrentTonerLevelsValidReturnsTonerLevels()
        {
            //setup
            var mockCoverageToolSet = new Mock<ICoverageToolset>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";

            var printer = new Printer()
            {
                printerId = printerId,
                printerName = "Test Printer",
                customerId = 1,
                isColour = true,
                cyanLowPercentage = 25,
                yellowLowPercentage = 25,
                magentaLowPercentage = 25,
                keyingLowPercentage = 25,
                stockLocationId = 1,
            };

            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd1!"),
                userId = 1,
                userLogin = userName
            };

            var customer = new Customer()
            {
                customerAddress = "Test Customer Address",
                customerContactNumber = "07700900000",
                customerID = printer.customerId,
                customerName = "Test Customer Name",
            };

            var tonerPrinter = new TonerPrinter
            {
                printerId = 1,
                tonerPercentage = 25,
                timestamp = new DateTime(2019, 02, 25)
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(printer.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>(){customer});
            mockTonerPrinterRepo.Setup(mTPR =>
                mTPR.GetTonerPrinterForDevice(printer.printerId, It.IsAny<CoverageToolset.ColorType>())).Returns(new List<TonerPrinter>(){tonerPrinter});

            var sut = new PrinterTonerHandler(mockCoverageToolSet.Object, mockCustomerRepo.Object,
                mockPrinterRepo.Object, mockTonerPrinterRepo.Object, mockUserRepo.Object);

            //action
            var res = (JsonNetResult)sut.GetCurrentTonerLevel(printerId, userName);

            //assert
            var expectedResult = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = tonerPrinter.tonerPercentage,
                Yellow = tonerPrinter.tonerPercentage,
                Magenta = tonerPrinter.tonerPercentage,
                Keying = tonerPrinter.tonerPercentage,
                PrinterID = printer.printerId
            };
            res.Data.Should().BeEquivalentTo(expectedResult);
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);

        }

    }

    

}