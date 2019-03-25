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
           
            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object)
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

            var mockContext = new Mock<ControllerContext>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>
                {new User() {hashedPassword = "junk", userId = 1, userLogin = userName}});

            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var sut = new StockLocationController(mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };
            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }
    }
}