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
    public class LoginControllerTests
    {
        [TestMethod()]
        public void LoginRequestWithValidCredentialsReturns200AndSetsSession()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";

            var login = new LoginModel {UserName = userName, Password = password};

            var mockLoginHandler = new Mock<ILoginHandler>();
            mockLoginHandler.Setup(m => m.LoginRequest(login)).Returns(new HttpStatusCodeResult(HttpStatusCode.OK));

            
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new MockSessionStateBase();
            mockControllerContext.Setup(m => m.HttpContext.Session).Returns(mockSession);

            var sut = new LoginController(mockLoginHandler.Object) {ControllerContext = mockControllerContext.Object};

            //action
            var res = sut.LoginRequest(login);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
            sut.Session["UserName"].Should().Be("Test");
        }

        [TestMethod()]
        public void LoginRequestWithInValidCredentialsReturns401AndDoesNotSetSession()
        {
            //setup
            const string userName = "Test";
            const string password = "DifferentPa$$w0rd!";

            var login = new LoginModel { UserName = userName, Password = password };

            var mockLoginHandler = new Mock<ILoginHandler>();
            mockLoginHandler.Setup(m => m.LoginRequest(login)).Returns(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));

            var mockSession = new MockSessionStateBase();
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(m => m.HttpContext.Session).Returns(mockSession);

            var sut = new LoginController(mockLoginHandler.Object) { ControllerContext = mockControllerContext.Object };

            //action
            var res = sut.LoginRequest(login);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod]
        public void LoginReturnsLoginView()
        {
            //setup
            var mockLoginHandler = new Mock<ILoginHandler>();
            
            var sut = new LoginController(mockLoginHandler.Object);

            //action
            var res = (ViewResult) sut.Login();

            //assert
            res.ViewName.Should().Be("Login");
            mockLoginHandler.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void LogoutReturnsLoginPage()
        {
            //setup
            var mockSession = new MockSessionStateBase {["UserName"] = "Test"};
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);

            var mockLoginHandler = new Mock<ILoginHandler>();

            var sut = new LoginController(mockLoginHandler.Object) {ControllerContext = mockContext.Object};

            //action
            var res = (ViewResult) sut.Logout();

            //assert
            res.ViewName.Should().Be("Login");
        }

    }
}