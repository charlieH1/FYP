using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Handlers;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagementTests.Handlers
{
    [TestClass()]
    public class RegistrationHandlerTests
    {
        [TestMethod()]
        public void RegisterUserValidNewUserReturnsHttpStatusCode200()
        {
            //setup
            var mockUserRepo = new Mock<IUserRepo>();
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "Pa$$w0rd!";
            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            mockUserRepo.Setup(m => m.GetUsers(userName)).Returns(new List<User>());

            var sut = new RegistrationHandler(mockUserRepo.Object);

            //action
            var res = sut.RegisterUser(model);

            //assert
            res.StatusCode.Should().Be(200);

        }

        [TestMethod()]
        public void RegisterUserConflictingUserNameReturnsHttpStatusCode409()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "Pa$$w0rd!";
            var user = new User
            {
                userLogin = userName, hashedPassword = Sodium.PasswordHash.ArgonHashString(password), userId = 1
            };
            var usersList = new List<User> {user};

            var mockUserRepo = new Mock<IUserRepo>();
            
            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            mockUserRepo.Setup(m => m.GetUsers(userName)).Returns(usersList);

            var sut = new RegistrationHandler(mockUserRepo.Object);

            //action
            var res = sut.RegisterUser(model);

            //assert
            res.StatusCode.Should().Be(409);

        }

        [TestMethod()]
        public void RegisterUserInvalidDetailsReturnsHttpStatusCode422()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "DifferentPa$$w0rd!";
            

            var mockUserRepo = new Mock<IUserRepo>();

            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            

            var sut = new RegistrationHandler(mockUserRepo.Object);

            //action
            var res = sut.RegisterUser(model);

            //assert
            res.StatusCode.Should().Be(422);
            mockUserRepo.VerifyNoOtherCalls();

        }
    }
}