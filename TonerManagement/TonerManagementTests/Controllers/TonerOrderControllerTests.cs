using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Controllers;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagementTests.TestHelpers;

namespace TonerManagementTests.Controllers
{
    [TestClass()]
    public class TonerOrderControllerTests
    {
        

        [TestMethod()]
        public void IndexInvalidUserReturnsStatusCodeForbidden()
        {
            //Setup
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();

            var mockSession = new MockSessionStateBase {["UserName"] = null};
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                mockStockLocationTonerHandler.Object) {ControllerContext = mockControllerContext.Object};

            //Action
            var res = (HttpStatusCodeResult) sut.Index();

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void IndexValidReturnsIndexPage()
        {
            //Setup
            const string userName = "Test";
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var user = new User()
            {
                hashedPassword = "testPassword",
                userId = 1,
                userLogin = userName
            };

            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User> {user});
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                    mockStockLocationTonerHandler.Object)
                { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (ViewResult)sut.Index();

            //Assert
            res.ViewName.Should().Be("Index");
        }


        [TestMethod()]
        public void GetTonerPercentageAndIdsForPrinterPerStockLocationInvalidUserReturnsStatusCodeForbidden()
        {
            //Setup
            const int stockLocationId = 1;
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();

            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                mockStockLocationTonerHandler.Object)
            { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetStockLocationAndTonerIdsForPrinterValidReturnsWhatHandlerReturns()
        {
            //Setup
            const string userName = "Test";
            const int stockLocationId = 1;
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var user = new User()
            {
                hashedPassword = "testPassword",
                userId = 1,
                userLogin = userName
            };
            var handlerReturn= new HttpStatusCodeResult(HttpStatusCode.OK);

            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User> { user });
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            mockDeviceHandler
                .Setup(mDH => mDH.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId, userName))
                .Returns(handlerReturn);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                    mockStockLocationTonerHandler.Object)
            { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult)sut.GetTonerPercentageAndIdsForPrintersPerStockLocation(stockLocationId);

            //Assert
            res.Should().BeEquivalentTo(handlerReturn);
        }


        [TestMethod()]
        public void CreateOrderInvalidUserReturnsStatusCodeForbidden()
        {
            //Setup
            const int stockLocationId = 1;
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();
            var tonerIds = new List<int> {1, 2, 3, 4};
            var quantities = new List<int> {1, 2, 1, 2};

            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                    mockStockLocationTonerHandler.Object)
                { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult) sut.CreateOrder(tonerIds, quantities, stockLocationId);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void CreateOrderListsDoNotMatchUpReturnsStatusCodeUnProcessableEntity()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();
            var tonerIds = new List<int> { 1, 2, 3 };
            var quantities = new List<int> { 1, 2, 1, 2 };
            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };

            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User> {user});
            
            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                    mockStockLocationTonerHandler.Object)
                { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult)sut.CreateOrder(tonerIds, quantities, stockLocationId);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Toner id's must be equal in length to the quantities"));
        }

        [TestMethod()]
        public void CreateOrderValidReturnsStatusWhatHandlerReturns()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            var mockUserHandler = new Mock<IUserHandler>();
            var mockDeviceHandler = new Mock<IDevicesHandler>();
            var mockStockLocationTonerHandler = new Mock<IStockLocationTonerHandler>();
            var mockControllerContext = new Mock<ControllerContext>();
            var tonerIds = new List<int> { 1, 2, 3, 4 };
            var quantities = new List<int> { 1, 2, 1, 2 };

            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };
            var handlerResult = new HttpStatusCodeResult(HttpStatusCode.OK);

            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User> { user });
            mockStockLocationTonerHandler.Setup(mSLTH => mSLTH.TonerOrder(It.IsAny<List<TonerOrderModel>>(), stockLocationId, userName))
                .Returns(handlerResult);

            var sut = new TonerOrderController(mockDeviceHandler.Object, mockUserHandler.Object,
                    mockStockLocationTonerHandler.Object)
                { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult)sut.CreateOrder(tonerIds, quantities, stockLocationId);

            //Assert
            res.Should().BeEquivalentTo(handlerResult);
        }
    }
}