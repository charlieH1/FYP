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
    public class StockLocationTonerHandlerTests
    {
        [TestMethod()]
        public void TonerOrderInvalidUserReturnsUnauthorizedStatusCode()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();

            var order = new List<TonerOrderModel>
            {
                new TonerOrderModel() {TonerId = 1, Quantity = 2},
                new TonerOrderModel() {TonerId = 2, Quantity = 1}
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>());

            var sut = new StockLocationTonerHandler(mockStockLocationTonerRepo.Object, mockUserRepo.Object,
                mockStockLocationRepo.Object, mockCustomerRepo.Object);

            //Action
            var res = sut.TonerOrder(order, stockLocationId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User Not found"));


        }

        
        [TestMethod()]
        public void TonerOrderInvalidStockLocationReturnsNotFoundStatusCode()
        {
            //Setup
            const int stockLocationId = 1;
            const string userName = "Test";
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();

            var order = new List<TonerOrderModel>
            {
                new TonerOrderModel() {TonerId = 1, Quantity = 2},
                new TonerOrderModel() {TonerId = 2, Quantity = 1}
            };

            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User>{user});
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId))
                .Returns((StockLocation) null);

            var sut = new StockLocationTonerHandler(mockStockLocationTonerRepo.Object, mockUserRepo.Object,
                mockStockLocationRepo.Object, mockCustomerRepo.Object);

            //Action
            var res = sut.TonerOrder(order, stockLocationId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "Stock Location not found"));


        }

        [TestMethod()]
        public void TonerOrderCustomerNotAccessibleByUserFailedReturnsUnauthorized()
        {
            //Setup
            const int stockLocationId = 1;
            const int customerId = 1;
            const string userName = "Test";
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();

            var order = new List<TonerOrderModel>
            {
                new TonerOrderModel() {TonerId = 1, Quantity = 2},
                new TonerOrderModel() {TonerId = 2, Quantity = 1}
            };

            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };
            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = customerId,
                stockLocationName = "Test stock location name",
                stockLocationAddress = "testStockLocationAddress",
                stockLocationContactNumber = "07700900000"
            };
            var customer = new Customer()
            {
                customerID = customerId,
                customerName = "Test customer",
                customerAddress = "Test Customer address",
                customerContactNumber = "07700900001"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>());

            var sut = new StockLocationTonerHandler(mockStockLocationTonerRepo.Object, mockUserRepo.Object,
                mockStockLocationRepo.Object, mockCustomerRepo.Object);

            //Action
            var res = sut.TonerOrder(order, stockLocationId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "This customer can not be accessed by this user."));


        }



        [TestMethod()]
        public void TonerOrderRepoSuccessfulReturnsOk()
        {
            //Setup
            const int stockLocationId = 1;
            const int customerId = 1;
            const string userName = "Test";
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();

            var order = new List<TonerOrderModel>
            {
                new TonerOrderModel() {TonerId = 1, Quantity = 2},
                new TonerOrderModel() {TonerId = 2, Quantity = 1}
            };

            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };
            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = customerId,
                stockLocationName = "Test stock location name",
                stockLocationAddress = "testStockLocationAddress",
                stockLocationContactNumber = "07700900000"
            };
            var customer = new Customer()
            {
                customerID = customerId,
                customerName = "Test customer",
                customerAddress = "Test Customer address",
                customerContactNumber = "07700900001"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer>{customer});
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.TonerOrder(order, stockLocationId)).Returns(true);

            var sut = new StockLocationTonerHandler(mockStockLocationTonerRepo.Object, mockUserRepo.Object,
                mockStockLocationRepo.Object, mockCustomerRepo.Object);

            //Action
            var res = sut.TonerOrder(order, stockLocationId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));


        }

        [TestMethod()]
        public void TonerOrderRepoUnsuccessfulReturns503()
        {
            //Setup
            const int stockLocationId = 1;
            const int customerId = 1;
            const string userName = "Test";
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();

            var order = new List<TonerOrderModel>
            {
                new TonerOrderModel() {TonerId = 1, Quantity = 2},
                new TonerOrderModel() {TonerId = 2, Quantity = 1}
            };

            var user = new User()
            {
                hashedPassword = "TestPassword",
                userId = 1,
                userLogin = userName
            };
            var stockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = customerId,
                stockLocationName = "Test stock location name",
                stockLocationAddress = "testStockLocationAddress",
                stockLocationContactNumber = "07700900000"
            };
            var customer = new Customer()
            {
                customerID = customerId,
                customerName = "Test customer",
                customerAddress = "Test Customer address",
                customerContactNumber = "07700900001"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(userName)).Returns(new List<User> { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(new List<Customer> { customer });
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.TonerOrder(order, stockLocationId)).Returns(false);

            var sut = new StockLocationTonerHandler(mockStockLocationTonerRepo.Object, mockUserRepo.Object,
                mockStockLocationRepo.Object, mockCustomerRepo.Object);

            //Action
            var res = sut.TonerOrder(order, stockLocationId, userName);

            //Assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(503, "An issue occured creating the order in the database"));


        }

    }
}