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
    public class ProfileControllerTests
    {
        

        [TestMethod()]
        public void IndexCalledWithValidUserReturnsIndexPageWithUserModel()
        {
            //setup
            const string username = "Test";
            const string password = "Pa$$w0rd";
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString(password),
                userId = 1,
                userLogin = username
            };
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(username)).Returns(new List<User> {user});
            var mockSession = new MockSessionStateBase { ["UserName"] = username };
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            var sut = new ProfileController(mockUserHandler.Object) {ControllerContext = mockControllerContext.Object};

            //action
            var res = (ViewResult) sut.Index();

            //assert
            res.ViewName.Should().Be("Index");
            res.Model.As<User>().Should().Be(user);

        }

        [TestMethod()]
        public void IndexCalledWithInValidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string username = null;
            const string password = "Pa$$w0rd";
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString(password),
                userId = 1,
                userLogin = username
            };
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(username)).Returns(new List<User> { user });
            var mockSession = new MockSessionStateBase { ["UserName"] = username };
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            var sut = new ProfileController(mockUserHandler.Object) { ControllerContext = mockControllerContext.Object };

            //action
            var res = (HttpStatusCodeResult)sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));

        }

        [TestMethod()]
        public void UpdateProfileRequestValidLoggedInUserReturnsWhatUserHandlerReturns()
        {

            //setup
            const string username = "TestUser";
            const string password = "Pa$$w0rd";
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString(password),
                userId = 1,
                userLogin = username
            };
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                CurrentPassword = "PA$$w0rd",
                NewPassword = "Pa$$w0rd!1",
                ConfirmNewPassword = "Pa$$w0rd!1",
                UserName = "NewUserName"
            };
            var userHandlerUpdateReturn = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(username)).Returns(new List<User> { user });
            mockUserHandler.Setup(mUH => mUH.UpdateUser(updateUser)).Returns(userHandlerUpdateReturn);
            var mockSession = new MockSessionStateBase { ["UserName"] = username };
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            var sut = new ProfileController(mockUserHandler.Object) { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult) sut.UpdateProfileRequest(updateUser);

            //assert
            res.Should().BeEquivalentTo(userHandlerUpdateReturn);
        }

        [TestMethod()]
        public void UpdateProfileRequestInValidLoggedInUserReturnsWithHttpStatusCodeUnauthorize()
        {

            //setup
            const string username = null;
            const string password = "Pa$$w0rd";
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString(password),
                userId = 1,
                userLogin = username
            };
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                CurrentPassword = "PA$$w0rd",
                NewPassword = "Pa$$w0rd!1",
                ConfirmNewPassword = "Pa$$w0rd!1",
                UserName = "NewUserName"
            };
            var userHandlerUpdateReturn = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(username)).Returns(new List<User> { user });
            mockUserHandler.Setup(mUH => mUH.UpdateUser(updateUser)).Returns(userHandlerUpdateReturn);
            var mockSession = new MockSessionStateBase { ["UserName"] = username };
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(mCC => mCC.HttpContext.Session).Returns(mockSession);
            var sut = new ProfileController(mockUserHandler.Object) { ControllerContext = mockControllerContext.Object };

            //Action
            var res = (HttpStatusCodeResult)sut.UpdateProfileRequest(updateUser);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }
    }
}