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
    }
}