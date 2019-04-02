using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Controllers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagementTests.TestHelpers;

namespace TonerManagementTests.Controllers
{
    [TestClass()]
    public class DevicesControllerTests
    {
        [TestMethod()]
        public void IndexValidUserReturnsPage()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object,mockDevicesHandler.Object,mockPrinterTonerHandler.Object )
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (ViewResult)sut.Index();

            //assert
            res.ViewName.Should().Be("Index");
        }

        [TestMethod()]
        public void IndexInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetDeviceDetailsInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetDeviceDetails(1);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetDeviceDetailsValidUserReturnsWhateverFromDeviceHandler()
        {
            //setup
            const string userName = "Test";
            const int printerId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var deviceHandlerResponse = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });
            mockDevicesHandler.Setup(mDH => mDH.GetDeviceDetails(printerId, userName)).Returns(deviceHandlerResponse);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetDeviceDetails(printerId);

            //assert
            res.Should().BeEquivalentTo(deviceHandlerResponse);
        }

        [TestMethod()]
        public void GetTonerPercentageInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentage(1);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }


        [TestMethod()]
        public void GetTonerPercentageValidUserReturnsWhateverFromPrinterTonerHandler()
        {
            //setup
            const string userName = "Test";
            const int printerId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var printerTonerHandlerResponse = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetCurrentTonerLevel(printerId, userName))
                .Returns(printerTonerHandlerResponse);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentage(printerId);

            //assert
            res.Should().BeEquivalentTo(printerTonerHandlerResponse);
        }

        [TestMethod()]
        public void UpdateTonerLowPercentageInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = 1
            };
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.UpdateTonerLow(request);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void UpdateTonerLowValidUserReturnsWhateverFromDeviceHandler()
        {
            //setup
            const string userName = "Test";
            const int printerId = 1;
            var request = new TonerPercentageAndPrinterIdModel()
            {
                Cyan = 25,
                Yellow = 25,
                Magenta = 25,
                Keying = 25,
                PrinterID = printerId
            };
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var deviceHandlerResponse = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockDevicesHandler = new Mock<IDevicesHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
            {new User() { hashedPassword = "junk", userId = 1, userLogin = userName}
            });
            mockDevicesHandler.Setup(mDH => mDH.UpdateTonerLowOnDevice(request, userName)).Returns(deviceHandlerResponse);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new DevicesController(mockUserHandler.Object, mockDevicesHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.UpdateTonerLow(request);

            //assert
            res.Should().BeEquivalentTo(deviceHandlerResponse);
        }

    }
}