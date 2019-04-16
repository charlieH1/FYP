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

        [TestMethod()]
        public void GetCustomerUserNotFoundReturnsHttpStatusCodeUnauthorized()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            const int customerId = 1;
            const string userName = "Test";
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());

            var sut = new CustomerHandler(mockCustomerRepo.Object,mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult) sut.GetCustomer(customerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetCustomerCustomerNotFoundReturnsHttpStatusCodeNotFound()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            const int customerId = 1;
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>(){user});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns((Customer) null);

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomer(customerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "Customer Not Found"));
        }

        [TestMethod()]
        public void GetCustomerCustomerNotAvailableForUserReturnsHttpStatusForbidden()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            const int customerId = 1;
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456789",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomer(customerId, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User not authorized to view customer"));
        }

        [TestMethod()]
        public void GetCustomerCustomerValidReturnsJsonWithCustomer()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            const int customerId = 1;
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456789",
                customerID = customerId,
                customerName = "TestCustomer"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>(){customer});

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (JsonResult)sut.GetCustomer(customerId, userName);

            //assert
            var expectedData = new {success = true, customer};
            res.Data.Should().BeEquivalentTo(expectedData);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void UpdateCustomerUserNotFoundReturnsHttpStatusCodeUnauthorized()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address 1"
            };
            const string userName = "Test";
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void UpdateCustomerCustomerNotFoundReturnsHttpStatusCodeNotFound()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address 1"
            };
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(request.CustomerId)).Returns((Customer)null);

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "Customer Not Found"));
        }

        [TestMethod()]
        public void UpdateCustomerCustomerNotAvailableForUserReturnsHttpStatusForbidden()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address 1"
            };
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456789",
                customerID = request.CustomerId,
                customerName = "TestCustomer"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(request.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User not authorized to view customer"));
        }

        [TestMethod()]
        public void UpdateCustomerDbSaveFailsReturnsHttpStatusInternalServerError()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address 1"
            };
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456789",
                customerID = request.CustomerId,
                customerName = "TestCustomer"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(request.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>(){customer});
            mockCustomerRepo.Setup(mCR => mCR.UpdateCustomer(request)).Returns(false);

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(500, "A Error occured updating the DB"));
        }

        [TestMethod()]
        public void UpdateCustomerValidReturnsHttpStatusOk()
        {
            //Setup
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address 1"
            };
            const string userName = "Test";
            var user = new User()
            {
                userId = 1,
                userLogin = userName,
                hashedPassword = "Junk"
            };
            var customer = new Customer()
            {
                customerAddress = "TestAddress1",
                customerContactNumber = "123456789",
                customerID = request.CustomerId,
                customerName = "TestCustomer"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(request.CustomerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>() { customer });
            mockCustomerRepo.Setup(mCR => mCR.UpdateCustomer(request)).Returns(true);

            var sut = new CustomerHandler(mockCustomerRepo.Object, mockUserRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request, userName);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}