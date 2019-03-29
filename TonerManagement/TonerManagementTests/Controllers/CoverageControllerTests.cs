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
    public class CoverageControllerTests
    {
        [TestMethod()]
        public void IndexCalledSessionContainsValidUserReturnsHomePage()
        {
            //setup
            const string userName = "Test";
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (ViewResult)sut.Index();

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

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
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

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetCoverageValidUserNameReturnsWhatPrinterTonerHandlerReturnsWhenGettingCoverage()
        {
            const string userName = "Test";
            var coverageRequest = new CoverageForCompanyRequestModel()
            {
                CoverageType = "DailyColor",
                CustomerId = 1,
                EndDate = new DateTime(2019, 02, 27),
                StartDate = new DateTime(2019, 02, 21)
            };
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetCoverage(coverageRequest, 1)).Returns(resultOfGetCoverage);

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverage(coverageRequest);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void GetCoverageInvalidUserNameReturnsHttpStatusCodeUnauthorized()
        {
            const string userName = "Test";
            var coverageRequest = new CoverageForCompanyRequestModel()
            {
                CoverageType = "DailyColor",
                CustomerId = 1,
                EndDate = new DateTime(2019, 02, 27),
                StartDate = new DateTime(2019, 02, 21)
            };
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>());
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetCoverage(coverageRequest, 1)).Returns(resultOfGetCoverage);

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverage(coverageRequest);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetGridCoverageValidUserNameReturnsWhatPrinterTonerHandlerReturnsWhenGettingCoverage()
        {
            const string userName = "Test";
            const int customerId = 1;
            var user = new User() { hashedPassword = "junk", userId = 1, userLogin = userName };
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
           
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {user});
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetCoverageGridForCustomer(user.userId, customerId)).Returns(resultOfGetCoverage);

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetGridCoverageForCustomer(customerId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void GetGridCoverageInvalidUserNameReturnsHttpStatusCodeUnauthorized()
        {
            const string userName = "Test";
            
            const int customerid = 1;
            
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>());
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetGridCoverageForCustomer(customerid);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetCoverageForPrinterValidUserNameReturnsWhatPrinterTonerHandlerReturnsWhenGettingCoverage()
        {
            const string userName = "Test";
            const int customerId = 1;
            var user = new User() { hashedPassword = "junk", userId = 1, userLogin = userName };
            var request = new CoverageForPrinterRequestModel()
            {
                CoverageType = "CyanMonthly",
                CustomerId = customerId,
                EndDate = DateTime.Today,
                PrinterId = 1,
                StartDate = DateTime.Today
            };
            var resultOfGetCoverage = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();

            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {user});
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            mockPrinterTonerHandler.Setup(mPTH => mPTH.GetCoverageForPrinter(request, user.userId)).Returns(resultOfGetCoverage);

            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverageForPrinter(request);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void GetForPrinterCoverageInvalidUserNameReturnsHttpStatusCodeUnauthorized()
        {
            const string userName = "Test";

            const int customerId = 1;

            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockUserHandler = new Mock<IUserHandler>();
            var mockPrinterTonerHandler = new Mock<IPrinterTonerHandler>();
            var mockContext = new Mock<ControllerContext>();
            var request = new CoverageForPrinterRequestModel()
            {
                CoverageType = "CyanMonthly",
                CustomerId = customerId,
                EndDate = DateTime.Today,
                PrinterId = 1,
                StartDate = DateTime.Today
            };
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>());
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);


            var sut = new CoverageController(mockUserHandler.Object, mockPrinterTonerHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCoverageForPrinter(request);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }
    }
}