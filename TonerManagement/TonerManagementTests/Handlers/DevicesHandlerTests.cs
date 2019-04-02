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
            var mockUserRepo = new Mock<IUserRepo>();

            const int printerId = 1;
            const string userName = "Test";
            mockPrinterRepo.Setup(mPR => mPR.GetPrinter(printerId)).Returns((Printer) null);


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

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


            var sut = new DevicesHandler(mockPrinterRepo.Object, mockCustomerRepo.Object, mockCoverageToolset.Object, mockUserRepo.Object);

            //action

            var res = (HttpStatusCodeResult)sut.UpdateTonerLowOnDevice(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Failed To update Db"));
        }
    }
}