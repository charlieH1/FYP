using System;
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
    public class UserHandlerTests
    {
        [TestMethod()]
        public void GetUsersReturnsUsers()
        {
            //setup
            var mockUserRepo = new Mock<IUserRepo>();
            const string userName = "Test";
            const string password = "Pa$$w0rd";
            var hashedPassword = Sodium.PasswordHash.ArgonHashString(password);
            var user = new User()
            {
                userId = 1,
                hashedPassword = hashedPassword,
                userLogin = userName
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>(){user});
            var sut = new UserHandler(mockUserRepo.Object);

            //action
            var res = sut.GetUsers(userName);

            //assert
            res.Count.Should().Be(1);
            res[0].Should().BeEquivalentTo(user);
        }

        [TestMethod()]
        public void UpdateUserWithNonExistentUserResultsInHttpStatusCodeNotFound()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!",
                UserName = "Test"
            };
            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns((User) null);
            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "The user was not found"));
        }

        [TestMethod()]
        public void UpdateUserWithInvalidCurrentPasswordResultsInHttpStatusCodeUnauthorized()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd1!"),
                userId = 1,
                userLogin = "Test"
            };

            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "The password or userId did not match"));
        }

        [TestMethod()]
        public void UpdateUserWithNoUserNameResultsInHttpStatusCodeUnprocessableEntity()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!",
                UserName = ""
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };

            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "The user name can not be null or empty"));
        }

        [TestMethod()]
        public void UpdateUserWithDifferentPasswordsResultsInHttpStatusCodeUnprocessableEntity()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!#",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };

            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "The new password and confirmation do not match"));
        }


        [TestMethod()]
        public void UpdateUserWithInsecurePasswordsResultsInHttpStatusCodeUnprocessableEntity()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Passw0rd",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Passw0rd",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };

            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "The new password does not match the security requirements"));
        }


        [TestMethod()]
        public void UpdateUserSuccessfulWithValidUserResultsInHttpStatusCodeOK()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };
            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);
            mockUserRepo.Setup(mUR => mUR.UpdateUser(It.IsAny<User>())).Returns(true);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void UpdateUserSuccessfulWithValidUserNoPasswordChangeResultsInHttpStatusCodeOK()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };
            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);
            mockUserRepo.Setup(mUR => mUR.UpdateUser(It.IsAny<User>())).Returns(true);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void UpdateUserUnsuccessfulWithValidUserResultsInHttpStatusCodeOK()
        {
            //Setup
            var mockUserRepo = new Mock<IUserRepo>();
            var updateUser = new UserUpdateModel()
            {
                UserId = 1,
                ConfirmNewPassword = "Pa$$w0rd1!",
                CurrentPassword = "Pa$$w0rd",
                NewPassword = "Pa$$w0rd1!",
                UserName = "Test"
            };
            var user = new User()
            {
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd"),
                userId = 1,
                userLogin = "Test"
            };
            
            mockUserRepo.Setup(mUR => mUR.GetUser(updateUser.UserId)).Returns(user);
            mockUserRepo.Setup(mUR => mUR.UpdateUser(It.IsAny<User>())).Returns(false);

            var sut = new UserHandler(mockUserRepo.Object);


            //Action
            var res = sut.UpdateUser(updateUser);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Failed to update DB"));
        }



    }
}