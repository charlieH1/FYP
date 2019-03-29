using System;
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
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexCalledSessionContainsValidUserReturnsHomePage()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase {["UserName"] = userName};
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new HomeController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (ViewResult) sut.Index();

            //assert
            res.ViewName.Should().Be("Index");
        }

        [TestMethod()]
        public void IndexCalledSessionContainsNoUserNameReturnsStatusCodeUnauthorized()
        {
            //setup
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new HomeController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void IndexCalledSessionContainsInValidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>());

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new HomeController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        

        

        [TestMethod()]
        public void GetTonerLowForCustomerWithValidCustomerReturnsGetTonerLow()
        {
            const string userName = "Test";
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetLowTonerLevelsOfCustomerPrinters(1, 1)).Returns(resultOfGetCoverage);

            var sut = new HomeController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetTonerLowForCustomer(1);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void GetTonerLowForCustomerWithInValidCustomerReturnsHttpStatusCodeUnauthorized()
        {
            const string userName = "Test";
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = null };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetLowTonerLevelsOfCustomerPrinters(1, 1)).Returns(resultOfGetCoverage);

            var sut = new HomeController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetTonerLowForCustomer(1);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }
    }
}