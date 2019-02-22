using System.Collections.Generic;
using System.Linq;
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
    public class CustomerHandlerTests
    {
        [TestMethod()]
        public void GetCustomersForUserTestWithValidUserNameAndCustomersAssignedReturnsCustomersJson()
        {
            //setup
            const string username = "Test";
            const string password = "Pa$$w0rd";
            const int userId = 1;
            var hashPassword = Sodium.PasswordHash.ArgonHashString(password);
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var userList = new List<User>
            {
                new User
                {
                    hashedPassword = hashPassword,
                    userId = userId,
                    userLogin = username
                }
            };
            var customerList = new List<Customer>
            {
                new Customer
                {
                    customerID = 1,
                    customerAddress = "test1Address",
                    customerContactNumber = "02079460000",
                    customerName = "Customer 1"
                },
                new Customer
                {
                    customerID = 2,
                    customerAddress = "test2Address",
                    customerContactNumber = "02079460111",
                    customerName = "testCustomer2"
                }
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(userList);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(customerList);

            var sut = new CustomerHandler(mockCustomerRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonResult) sut.GetCustomersForUser(username);

            //assert
            res.Data.Should().BeEquivalentTo(customerList.AsEnumerable());
        }

        [TestMethod()]
        public void GetCustomersForUserTestWithInvalidUserNameReturnsUnauthorized()
        {
            //setup

            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockUserRepo.Setup(mUR => mUR.GetUsers("invalid")).Returns(new List<User>());

            var sut = new CustomerHandler(mockCustomerRepo.Object,mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomersForUser("invalid");

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
            mockCustomerRepo.VerifyNoOtherCalls();
        }

        [TestMethod()]
        public void GetCustomersForUserTestWithValidUserNameAndNoCustomersAssignedReturnsEmptyJson()
        {
            //setup
            const string username = "Test";
            const string password = "Pa$$w0rd";
            const int userId = 1;
            var hashPassword = Sodium.PasswordHash.ArgonHashString(password);
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var userList = new List<User>
            {
                new User
                {
                    hashedPassword = hashPassword,
                    userId = userId,
                    userLogin = username
                }
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(userList);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(userId)).Returns(new List<Customer>());

            var sut = new CustomerHandler(mockCustomerRepo.Object,mockUserRepo.Object);

            //action
            var res = (JsonResult)sut.GetCustomersForUser(username);

            //assert
            res.Data.Should().BeEquivalentTo(new List<Customer>().AsEnumerable());
        }
    }
}