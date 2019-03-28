using Microsoft.VisualStudio.TestTools.UnitTesting;
using TonerManagement.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagementTests.TestHelpers;

namespace TonerManagement.Controllers.Tests
{
    [TestClass()]
    public class StockLocationControllerTests
    {
        [TestMethod()]
        public void IndexRequestUserLoggedInReturnsIndexPage()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
           
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object,mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (ViewResult)sut.Index();

            //assert
            res.ViewName.Should().Be("Index");
        }

        [TestMethod()]
        public void IndexRequestNoUserLoggedInReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object,mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetStockLocationsForCustomerWithValidUserReturnsStockHandlerResult()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
            const int customerId = 1;

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.OK,"This is a test");
            mockStockLocationHandler.Setup(mSLH => mSLH.GetStockLocationsForCustomer(customerId, userName)).Returns(expectedResult);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocationsForCustomer(customerId);

            //assert
            res.Should().Be(expectedResult);
        }

        [TestMethod()]
        public void GetStockLocationsForCustomerWithNoLoggedInUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            const int customerId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocationsForCustomer(customerId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }


        [TestMethod()]
        public void GetStockLocationWithValidUserReturnsStockHandlerResult()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
            const int stockLocationId = 1;

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.OK, "This is a test");
            mockStockLocationHandler.Setup(mSLH => mSLH.GetStockLocation(stockLocationId, userName)).Returns(expectedResult);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocation(stockLocationId);

            //assert
            res.Should().Be(expectedResult);
        }

        [TestMethod()]
        public void GetStockLocationWithNoLoggedInUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            const int stockLocationId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocation(stockLocationId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetTonerStockLocationForGridWithValidUserReturnsStockHandlerResult()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
            const int stockLocationId = 1;

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.OK, "This is a test");
            mockStockLocationHandler.Setup(mSLH => mSLH.GetTonerStockLocationForGrid(stockLocationId, userName)).Returns(expectedResult);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetTonerStockLocationForStockLocationForGrid(stockLocationId);

            //assert
            res.Should().Be(expectedResult);
        }

        [TestMethod()]
        public void GetTonerStockLocationForGridWithNoLoggedInUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            const int stockLocationId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.GetTonerStockLocationForStockLocationForGrid(stockLocationId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }


        [TestMethod()]
        public void UpdateStockLocationWithValidUserReturnsStockHandlerResult()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
            var updateStockLocationObject = new UpdatedStockLocationModel()
            {
                StockLocationAddress = "test Address",
                StockLocationContactNumber = "123456",
                StockLocationId = 1,
                StockLocationName = "Test"
            };

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.OK, "This is a test");
            mockStockLocationHandler.Setup(mSLH => mSLH.UpdateStockLocation(updateStockLocationObject, userName)).Returns(expectedResult);

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updateStockLocationObject);

            //assert
            res.Should().Be(expectedResult);
        }

        [TestMethod()]
        public void UpdateStockLocationWithNoLoggedInUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            const int stockLocationId = 1;
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockStockLocationHandler = new Mock<IStockLocationHandler>();
            var updateStockLocationObject = new UpdatedStockLocationModel()
            {
                StockLocationAddress = "test Address",
                StockLocationContactNumber = "123456",
                StockLocationId = 1,
                StockLocationName = "Test"
            };

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object, mockStockLocationHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updateStockLocationObject);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }



    }
}