using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Handlers;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagementTests.Handlers
{
    [TestClass()]
    public class LoginHandlerTests
    {
        [TestMethod()]
        public void LoginRequestValidUserReturns200()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            var hashedPassword = Sodium.PasswordHash.ArgonHashString(password);

            var user = new User {userLogin = userName, hashedPassword = hashedPassword, userId = 1};
            var login = new LoginModel {UserName = userName, Password = password};

            var userList = new List<User> {user};

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(m => m.GetUsers(userName)).Returns(userList);
            var sut = new LoginHandler(mockUserRepo.Object);
            
            //action
            var res = sut.LoginRequest(login);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod]
        public void LoginRequestInValidUserPasswordReturns401()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            var hashedPassword = Sodium.PasswordHash.ArgonHashString("ADifferentPa$$w0rd");

            var user = new User { userLogin = userName, hashedPassword = hashedPassword, userId = 1 };
            var login = new LoginModel { UserName = userName, Password = password };

            var userList = new List<User> { user };

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(m => m.GetUsers(userName)).Returns(userList);
            var sut = new LoginHandler(mockUserRepo.Object);

            //action
            var res = sut.LoginRequest(login);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod]
        public void LoginRequestInValidUserDetailsReturns401()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            

            
            var login = new LoginModel { UserName = userName, Password = password };

            

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(m => m.GetUsers(userName)).Returns(new List<User>());
            var sut = new LoginHandler(mockUserRepo.Object);

            //action
            var res = sut.LoginRequest(login);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }
    }
}