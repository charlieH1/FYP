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
    public class DevicesHandlerTests
    {
        [TestMethod()]
        public void GetDeviceDetailsPrinterNotFoundReturnsHttpStatusCodeUnknown()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns((Printer) null);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object,mockTonerPrinterRepo.Object,mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult) sut.GetDeviceDetails(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, "Printer couldn't be found"));



        }
        [TestMethod()]
        public void GetDeviceDetailsUserNotFoundReturnsHttpStatusCodeUnknown()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";
            var printer  = new Printer()
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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object,mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.GetDeviceDetails(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, userName + " User not found"));
        }

        [TestMethod()]
        public void GetDeviceDetailsCustomerNotAvailableForUserReturnsHttpStatusCodeForbiden()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object,mockTonerPrinterRepo.Object,mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.GetDeviceDetails(printerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));
        }

        [TestMethod()]
        public void GetDeviceDetailsColorPrinterReturnsDetails()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            var expectedPrinter = new PrinterResponseModelWithAverageTonerCoverage()
            {
                AverageTonerCoverage = 28.0d,
                CyanLowTonerPercentage = printer.cyanLowPercentage,
                YellowLowTonerPercentage = printer.yellowLowPercentage,
                MagentaLowTonerPercentage = printer.magentaLowPercentage,
                KeyingLowTonerPercentage = printer.keyingLowPercentage,
                PrinterId = printer.printerId,
                PrinterName = printer.printerName
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() {user});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>(){customer});
            mockCoverageToolset.Setup(mCT =>
                    mCT.CalculateAverageCoverageForWholeLife(printer.printerId, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(28.0d);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (JsonResult) sut.GetDeviceDetails(printerId, userName);

            //assert
            var expectedData = new {success = true, printer = expectedPrinter, customer};
            res.Data.Should().BeEquivalentTo(expectedData);
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
            mockCoverageToolset.Verify(mCT => mCT.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.C));
            mockCoverageToolset.Verify(mCT => mCT.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.Y));
            mockCoverageToolset.Verify(mCT => mCT.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.M));
            mockCoverageToolset.Verify(mCT => mCT.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.K));
        }

        [TestMethod()]
        public void GetDeviceDetailsBAndWPrinterReturnsDetails()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";
            var printer = new Printer()
            {
                printerId = printerId,
                printerName = "Test Printer",
                customerId = 1,
                isColour = false,
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
            var expectedPrinter = new PrinterResponseModelWithAverageTonerCoverage()
            {
                AverageTonerCoverage = 28.0d,
                CyanLowTonerPercentage = printer.cyanLowPercentage,
                YellowLowTonerPercentage = printer.yellowLowPercentage,
                MagentaLowTonerPercentage = printer.magentaLowPercentage,
                KeyingLowTonerPercentage = printer.keyingLowPercentage,
                PrinterId = printer.printerId,
                PrinterName = printer.printerName
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>() { customer });
            mockCoverageToolset.Setup(mCT =>
                    mCT.CalculateAverageCoverageForWholeLife(printer.printerId, CoverageToolset.ColorType.K))
                .Returns(28.0d);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (JsonResult)sut.GetDeviceDetails(printerId, userName);

            //assert
            var expectedData = new { success = true, printer = expectedPrinter, customer };
            res.Data.Should().BeEquivalentTo(expectedData);
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
            mockCoverageToolset.Verify(mCT => mCT.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.K));
            mockCoverageToolset.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void UpdateTonerLowPercentagePrinterNotFoundReturnsHttpStatusCodeUnknown()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns((Printer)null);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, "Printer couldn't be found"));



        }
        [TestMethod()]
        public void UpdateTonerLowPercentageUserNotFoundReturnsHttpStatusCodeUnknown()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, userName + " User not found"));
        }

        [TestMethod()]
        public void UpdateTonerLowePercentageCustomerNotAvailableForUserReturnsHttpStatusCodeForbiden()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));
        }

        [TestMethod()]
        public void UpdateTonerLowPercentageValidReturnsHttpStatusCodeOk()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>(){customer});
            mockPrinterRepo.Setup(mPR => mPR.UpdatePrinter(It.IsAny<Printer>())).Returns(true);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object,mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void UpdateTonerLowPercentageDbFailReturnsHttpStatusCode422()
        {
            //setup
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
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
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns(printer);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customer.customerID)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>() { customer });
            mockPrinterRepo.Setup(mPR => mPR.UpdatePrinter(It.IsAny<Printer>())).Returns(false);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Failed To update Db"));
        }

        [TestMethod()]
        public void GetDetailedPrinterGridNoUserReturnsHttpStatusCodeNotFound()
        {
            //setup
            const string userName = "test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());
            
            var sut = new DevicesHandler(mockPrinterRepo.Object,mockCustomerRepo.Object,mockCoverageToolset.Object,mockUserRepo.Object,mockTonerPrinterRepo.Object,mockStockLocationRepo.Object);

            //Action
            var res = (HttpStatusCodeResult)sut.GetDetailedPrinterGrid(customerId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, userName + " User not found"));
        }

        [TestMethod()]
        public void GetDetailedPrinterGridCustomerNotFoundReturnsHttpStatusCodeNotFound()
        {
            //setup
            const string userName = "test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var user = new User()
            {
                hashedPassword = "Blah",
                userId = 1,
                userLogin = userName,
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> {user});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns((Customer) null);

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (HttpStatusCodeResult) sut.GetDetailedPrinterGrid(customerId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, "Customer not found"));

        }

        [TestMethod()]
        public void GetDetailedPrinterGridCustomerNotAccessibleByUserReturnsHttpStatusCodeNotFound()
        {
            //setup
            const string userName = "test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var user = new User()
            {
                hashedPassword = "Blah",
                userId = 1,
                userLogin = userName,
            };
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "07700900000",
                customerID = customerId,
                customerName = "test customer",
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (HttpStatusCodeResult)sut.GetDetailedPrinterGrid(customerId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));

        }

        [TestMethod()]
        public void GetDetailedPrinterGridValidReturnsDetailsForGrid()
        {
            //setup
            const string userName = "test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var user = new User()
            {
                hashedPassword = "Blah",
                userId = 1,
                userLogin = userName,
            };
            var customer = new Customer()
            {
                customerAddress = "testAddress1",
                customerContactNumber = "07700900000",
                customerID = customerId,
                customerName = "test customer",
            };

            var printers = new List<Printer>
            {
                new Printer()
                {
                    customerId = customerId,
                    isColour = true,
                    printerId = 1,
                    stockLocationId = 1,
                    cyanLowPercentage = 25,
                    yellowLowPercentage = 25,
                    magentaLowPercentage = 25,
                    keyingLowPercentage = 25,
                    printerName = "colourPrinter"
                },
                new Printer()
                {
                    customerId = customerId,
                    isColour = false,
                    printerId = 2,
                    stockLocationId = 1,
                    keyingLowPercentage = 25,
                    printerName = "B&WPrinter"
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


            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>{customer});
            mockPrinterRepo.Setup(mPR => mPR.GetPrintersFromCustomer(customerId)).Returns(printers);
            mockCoverageToolset
                .Setup(mCT => mCT.CalculateAverageCoverageForWholeLife(1, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(28.0d);
            mockCoverageToolset.Setup(mCT => mCT.CalculateAverageCoverageForWholeLife(2, CoverageToolset.ColorType.K))
                .Returns(28.0d);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(tonerPrinter1);

            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter2);
            

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (JsonResult)sut.GetDetailedPrinterGrid(customerId, userName);

            //Assert
            var expectedListModel = new List<HighDetailPrinterModel>
            {
                new HighDetailPrinterModel()
                {
                    AverageCoverage = 28.0d,
                    CyanCoverage = 28.0d,
                    CyanLevel = 25,
                    KeyingCoverage = 28.0d,
                    KeyingLevel = 25,
                    MagentaCoverage = 28.0d,
                    MagentaLevel = 25,
                    PrinterId = printers[0].printerId,
                    PrinterName = printers[0].printerName,
                    YellowCoverage = 28.0d,
                    YellowLevel = 25,
                },
                new HighDetailPrinterModel()
                {
                    AverageCoverage = 28.0d,
                    KeyingLevel = 30,
                    KeyingCoverage = 28.0d,
                    PrinterId = printers[1].printerId,
                    PrinterName = printers[1].printerName
                }
            };
            res.Data.Should().BeEquivalentTo(expectedListModel);
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);

        }

        [TestMethod()]
        public void GetTonerPercentageAndIdsForPrinterPerStockLocation_StockLocationDoesNotExist_ReturnsHttpStatusCodeNotFound()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns((StockLocation) null);

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);
            
            //Action
            var res = (HttpStatusCodeResult) sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId,
                userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, "Stock Location could not be found"));
        }


        [TestMethod()]
        public void GetTonerPercentageAndIdsForPrinterPerStockLocation_UserDoesNotExist_ReturnsHttpStatusCodeNotFound()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                stockLocationName = "Test Stock Location",
                stockLocationAddress = "Test Stock Location Address",
                stockLocationContactNumber = "07700900000",
                customerId = customerId
            };
            

            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(stockLocation);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId,
                userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(404, userName + " User not found"));
        }

        [TestMethod()]
        public void GetTonerPercentageAndIdsForPrinterPerStockLocation_CustomerNotAccesibleByUser_ReturnsHttpStatusCodeNotFound()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                stockLocationName = "Test Stock Location",
                stockLocationAddress = "Test Stock Location Address",
                stockLocationContactNumber = "07700900000",
                customerId = customerId
            };
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "blah"
            };
            var customer = new Customer()
            {
                customerID = customerId,
                customerName = "Test Customer",
                customerAddress = "TestAddress",
                customerContactNumber = "07700900001"
            };

            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(stockLocation);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId,
                userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));
        }

        [TestMethod()]
        public void GetTonerPercentageAndIdsForPrinterPerStockLocation_Valid_DetailsReturned()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            const int customerId = 1;
            var mockPrinterRepo = new Mock<IPrinterRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockCoverageToolset = new Mock<ICoverageToolset>();
            var mockTonerPrinterRepo = new Mock<ITonerPrinterRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                stockLocationName = "Test Stock Location",
                stockLocationAddress = "Test Stock Location Address",
                stockLocationContactNumber = "07700900000",
                customerId = customerId
            };
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "blah"
            };
            var customer = new Customer()
            {
                customerID = customerId,
                customerName = "Test Customer",
                customerAddress = "TestAddress",
                customerContactNumber = "07700900001"
            };
            var printers = new List<Printer>
            {
                new Printer()
                {
                    customerId = customerId,
                    isColour = true,
                    printerId = 1,
                    stockLocationId = 1,
                    cyanLowPercentage = 25,
                    yellowLowPercentage = 25,
                    magentaLowPercentage = 25,
                    keyingLowPercentage = 25,
                    printerName = "colourPrinter"
                },
                new Printer()
                {
                    customerId = customerId,
                    isColour = false,
                    printerId = 2,
                    stockLocationId = 1,
                    keyingLowPercentage = 25,
                    printerName = "B&WPrinter"
                }
            };

            var tonerPrinter1 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 25,
                    tonerId = 1,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 1,
                    tonerPercentage = 28,
                    tonerId = 2,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            var tonerPrinter2 = new List<TonerPrinter>
            {
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 30,
                    tonerId = 3,
                    timestamp = new DateTime(2019, 02, 25)
                },
                new TonerPrinter
                {
                    printerId = 2,
                    tonerPercentage = 32,
                    tonerId = 4,
                    timestamp = new DateTime(2019, 02, 21)
                }
            };

            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(stockLocation);
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>{customer});
            mockPrinterRepo.Setup(mPR => mPR.GetPrinterFromStockLocation(stockLocationId)).Returns(printers);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(1, It.IsAny<CoverageToolset.ColorType>()))
                .Returns(tonerPrinter1);
            mockTonerPrinterRepo.Setup(mTPR => mTPR.GetTonerPrinterForDevice(2, CoverageToolset.ColorType.K))
                .Returns(tonerPrinter2);

            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object,
                mockUserRepo.Object, mockTonerPrinterRepo.Object, mockStockLocationRepo.Object);

            //Action
            var res = (JsonResult)sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId,
                userName);

            //Assert
            var expectedResult = new List<TonerIdTonerPercentageAndPrinterModel>
            {
                new TonerIdTonerPercentageAndPrinterModel()
                {
                    CyanId = 1,
                    CyanPercentage = 25,
                    DeviceId = printers[0].printerId,
                    KeyingId = 1,
                    KeyingPercentage = 25,
                    MagentaId = 1,
                    MagentaPercentage = 25,
                    YellowId = 1,
                    YellowPercentage = 25
                },
                new TonerIdTonerPercentageAndPrinterModel()
                {
                    KeyingId = 3,
                    KeyingPercentage = 30,
                    DeviceId = printers[1].printerId
                }
            };
            res.Data.Should().BeEquivalentTo(expectedResult);
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }
    }
}