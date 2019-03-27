using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Handlers;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagementTests.Handlers
{
    [TestClass()]
    public class StockLocationHandlerTests
    {
        [TestMethod()]
        public void GetStockLocationsForCustomerInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const int customerId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>());

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult) sut.GetStockLocationsForCustomer(customerId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known"));
            
        }

        [TestMethod()]
        public void GetStockLocationsForCustomerUserHasNoAccessToCustomerReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const int customerId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var wantedCustomer = new Customer()
            {
                customerID = customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>(){user});
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocationsForCustomer(customerId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));

        }

        [TestMethod()]
        public void GetStockLocationsForCustomerUserValidAndHasAccessToCustomerReturnsJsonContainingStockLocations()
        {
            //setup
            const int customerId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var wantedCustomer = new Customer()
            {
                customerID = customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };
            var stockLocations = new List<StockLocation>()
            {
                new StockLocation()
                {
                    customerId = customerId,
                    stockLocationAddress = "StockLocationAddress1",
                    stockLocationContactNumber = "123456",
                    stockLocationId = 1,
                    stockLocationName = "Test Location1"
                },
                new StockLocation()
                {
                    customerId = customerId,
                    stockLocationAddress = "StockLocationAddress2",
                    stockLocationContactNumber = "12345678",
                    stockLocationId = 2,
                    stockLocationName = "Test stock location2"
                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocationsForCustomer(customerId)).Returns(stockLocations);

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetStockLocationsForCustomer(customerId, username);

            //assert
            res.Data.Should().BeEquivalentTo(stockLocations);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);

        }


        [TestMethod()]
        public void GetStockLocationInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>());

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocation(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known"));

        }

        [TestMethod()]
        public void GetStockLocationNotAStockLocationReturnsHttpStatusCodeNotFound()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
           

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns((StockLocation) null);

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocation(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "The stock location could not be found"));

        }

        [TestMethod()]
        public void GetStockLocationUserHasntGotAccessToCustomerReturnsHttpStatusCodeNotForbidden()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetStockLocation(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));

        }

        [TestMethod()]
        public void GetStockLocationUserHasGotAccessToCustomerReturnsJsonContainingStockLocation()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetStockLocation(stockLocationId, username);

            //assert
            var data = new {success = true, StockLocation = wantedStockLocation};
            res.Data.Should().BeEquivalentTo(data);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void UpdateStockLocationUserNotKnownReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "Test StockLocationName",
                StockLocationAddress = "TestStockLocationAddress",
                StockLocationContactNumber = "1234567"
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>());

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known"));
        }


        [TestMethod()]
        public void UpdateStockLocationStockLocationNotFoundReturnsHttpStatusCodeNotFound()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "Test StockLocationName",
                StockLocationAddress = "TestStockLocationAddress",
                StockLocationContactNumber = "1234567"
            };
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>(){user});
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns((StockLocation) null);

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "The stock location could not be found"));
        }

        [TestMethod()]
        public void UpdateStockLocationUserHasNoAccessToCustomerReturnsHttpStatusCodeForbidden()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "Test StockLocationName",
                StockLocationAddress = "TestStockLocationAddress",
                StockLocationContactNumber = "07700900002"
            };
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900000",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));
        }

        

        [TestMethod()]
        public void UpdateStockLocationEmptyStockLocationAddressReturnsHttpStatusCodeUnprocessableEntity()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "Test StockLocationName",
                StockLocationAddress = "",
                StockLocationContactNumber = "07700900002"
            };
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900000",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                customer,
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Stock Location Address cant be null or whitespace"));
        }

        [TestMethod()]
        public void UpdateStockLocationEmptyStockLocationNameReturnsHttpStatusCodeUnprocessableEntity()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "",
                StockLocationAddress = "TestStockLocation Address",
                StockLocationContactNumber = "07700900002"
            };
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900000",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                customer,
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Stock Location Name cant be null or whitespace"));
        }

        [TestMethod()]
        public void UpdateStockLocationEmptyStockLocationContactNumberReturnsHttpStatusCodeUnprocessableEntity()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "TestStockLocationName1",
                StockLocationAddress = "TestStockLocation Address",
                StockLocationContactNumber = ""
            };
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900000",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                customer,
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Stock Location phone number cant be null or whitespace"));
        }

        [TestMethod()]
        public void UpdateStockLocationValidStockLocationDbUpdateReturnsHttpStatusCodeOk()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "TestStockLocationName1",
                StockLocationAddress = "TestStockLocation Address",
                StockLocationContactNumber = "07700900001"
            };
            
            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900004",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                customer,
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationRepo.Setup(mSLR => mSLR.UpdateStockLocation(It.IsAny<StockLocation>())).Returns(true);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [TestMethod()]
        public void UpdateStockLocationInvalidStockLocationDbUpdateReturnsHttpStatusCodeOk()
        {
            //setup
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();
            var updatedStockLocationModel = new UpdatedStockLocationModel()
            {
                StockLocationId = 1,
                StockLocationName = "TestStockLocationName1",
                StockLocationAddress = "TestStockLocation Address",
                StockLocationContactNumber = "07700900001"
            };

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };
            var stockLocation = new StockLocation()
            {
                customerId = 1,
                stockLocationAddress = "Test stock location address",
                stockLocationContactNumber = "07700900004",
                stockLocationName = "name prior change",
                stockLocationId = updatedStockLocationModel.StockLocationId
            };
            var customer = new Customer()
            {
                customerID = stockLocation.customerId,
                customerAddress = "TestCustomerAddress",
                customerContactNumber = "07700900001",
                customerName = "TestCustomerName"
            };
            var customerList = new List<Customer>()
            {
                customer,
                new Customer()
                {
                    customerAddress = "TestCustomerAddress2",
                    customerContactNumber = "07700900004",
                    customerID = 2,
                    customerName = "TestCustomer2"

                }
            };
            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(updatedStockLocationModel.StockLocationId))
                .Returns(stockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(stockLocation.customerId)).Returns(customer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationRepo.Setup(mSLR => mSLR.UpdateStockLocation(It.IsAny<StockLocation>())).Returns(false);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.UpdateStockLocation(updatedStockLocationModel, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422, "Failed to update DB"));
        }

        [TestMethod()]
        public void GetTonerStockLocationForGridUnknownUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>());

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "User not known"));
        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridNotAStockLocationReturnsHttpStatusCodeNotFound()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };


            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns((StockLocation)null);

            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.NotFound, "The stock location could not be found"));

        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridUserHasntGotAccessToCustomerReturnsHttpStatusCodeNotForbidden()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);


            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (HttpStatusCodeResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer"));

        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridCyanColorTonerReturnsJsonDataResult()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            var stockLocationToners = new List<StockLocationToner>()
            {
                new StockLocationToner()
                {
                    stockLocationId = stockLocationId,
                    stockLocationTonerId = 1,
                    quantity = 5,
                    tonerId = 1,
                }
            };
            var toner = new Toner()
            {
                isCyan = true,
                isKeying = false,
                isMagenta = false,
                isYellow = false,
                tonerCode = "TestTonerCode",
                tonerId = stockLocationToners[0].tonerId,
                tonerName = "testTonerName"
            };

            var expectedGridData = new List<TonerStockLocationGridModel>()
            {
                new TonerStockLocationGridModel()
                {
                    Color = "Cyan",
                    Quantity = stockLocationToners[0].quantity,
                    TonerId = stockLocationToners[0].tonerId,
                    TonerName = toner.tonerName
                }
            };
            

            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.GetStockLocationTonersForStockLocation(stockLocationId))
                .Returns(stockLocationToners);
            mockTonerRepo.Setup(mTR => mTR.GetToner(stockLocationToners[0].tonerId)).Returns(toner);



            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Data.Should().BeEquivalentTo(expectedGridData);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);

        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridYellowColorTonerReturnsJsonDataResult()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            var stockLocationToners = new List<StockLocationToner>()
            {
                new StockLocationToner()
                {
                    stockLocationId = stockLocationId,
                    stockLocationTonerId = 1,
                    quantity = 5,
                    tonerId = 1,
                }
            };
            var toner = new Toner()
            {
                isCyan = false,
                isKeying = false,
                isMagenta = false,
                isYellow = true,
                tonerCode = "TestTonerCode",
                tonerId = stockLocationToners[0].tonerId,
                tonerName = "testTonerName"
            };

            var expectedGridData = new List<TonerStockLocationGridModel>()
            {
                new TonerStockLocationGridModel()
                {
                    Color = "Yellow",
                    Quantity = stockLocationToners[0].quantity,
                    TonerId = stockLocationToners[0].tonerId,
                    TonerName = toner.tonerName
                }
            };


            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.GetStockLocationTonersForStockLocation(stockLocationId))
                .Returns(stockLocationToners);
            mockTonerRepo.Setup(mTR => mTR.GetToner(stockLocationToners[0].tonerId)).Returns(toner);



            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Data.Should().BeEquivalentTo(expectedGridData);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);

        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridMagentaColorTonerReturnsJsonDataResult()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            var stockLocationToners = new List<StockLocationToner>()
            {
                new StockLocationToner()
                {
                    stockLocationId = stockLocationId,
                    stockLocationTonerId = 1,
                    quantity = 5,
                    tonerId = 1,
                }
            };
            var toner = new Toner()
            {
                isCyan = false,
                isKeying = false,
                isMagenta = true,
                isYellow = false,
                tonerCode = "TestTonerCode",
                tonerId = stockLocationToners[0].tonerId,
                tonerName = "testTonerName"
            };

            var expectedGridData = new List<TonerStockLocationGridModel>()
            {
                new TonerStockLocationGridModel()
                {
                    Color = "Magenta",
                    Quantity = stockLocationToners[0].quantity,
                    TonerId = stockLocationToners[0].tonerId,
                    TonerName = toner.tonerName
                }
            };


            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.GetStockLocationTonersForStockLocation(stockLocationId))
                .Returns(stockLocationToners);
            mockTonerRepo.Setup(mTR => mTR.GetToner(stockLocationToners[0].tonerId)).Returns(toner);



            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Data.Should().BeEquivalentTo(expectedGridData);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);

        }

        [TestMethod()]
        public void GetTonerStockLocationsForGridKeyingColorTonerReturnsJsonDataResult()
        {
            //setup
            const int stockLocationId = 1;
            const string username = "Test";
            var mockStockLocationRepo = new Mock<IStockLocationRepo>();
            var mockCustomerRepo = new Mock<ICustomerRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockTonerRepo = new Mock<ITonerRepo>();
            var mockStockLocationTonerRepo = new Mock<IStockLocationTonerRepo>();

            var user = new User()
            {
                userId = 1,
                userLogin = username,
                hashedPassword = Sodium.PasswordHash.ArgonHashString("Pa$$w0rd!")
            };

            var wantedStockLocation = new StockLocation()
            {
                stockLocationId = stockLocationId,
                customerId = 1,
                stockLocationAddress = "TestStockLocationAddress1",
                stockLocationName = "TestStockLocationName",
                stockLocationContactNumber = "123456"
            };
            var wantedCustomer = new Customer()
            {
                customerID = wantedStockLocation.customerId,
                customerAddress = "TestAddress",
                customerContactNumber = "123465",
                customerName = "TestCustomer1"
            };
            var customerList = new List<Customer>()
            {
                wantedCustomer,
                new Customer()
                {
                    customerAddress = "TestAddress2",
                    customerContactNumber = "654321",
                    customerID = 2,
                    customerName = "TestCustomer2"
                }
            };

            var stockLocationToners = new List<StockLocationToner>()
            {
                new StockLocationToner()
                {
                    stockLocationId = stockLocationId,
                    stockLocationTonerId = 1,
                    quantity = 5,
                    tonerId = 1,
                }
            };
            var toner = new Toner()
            {
                isCyan = false,
                isKeying = true,
                isMagenta = false,
                isYellow = false,
                tonerCode = "TestTonerCode",
                tonerId = stockLocationToners[0].tonerId,
                tonerName = "testTonerName"
            };

            var expectedGridData = new List<TonerStockLocationGridModel>()
            {
                new TonerStockLocationGridModel()
                {
                    Color = "Keying",
                    Quantity = stockLocationToners[0].quantity,
                    TonerId = stockLocationToners[0].tonerId,
                    TonerName = toner.tonerName
                }
            };


            mockUserRepo.Setup(mUR => mUR.GetUsers(username)).Returns(new List<User>() { user });
            mockStockLocationRepo.Setup(mSLR => mSLR.GetStockLocation(stockLocationId)).Returns(wantedStockLocation);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomer(wantedStockLocation.customerId)).Returns(wantedCustomer);
            mockCustomerRepo.Setup(mCR => mCR.GetCustomersForUser(user.userId)).Returns(customerList);
            mockStockLocationTonerRepo.Setup(mSLTR => mSLTR.GetStockLocationTonersForStockLocation(stockLocationId))
                .Returns(stockLocationToners);
            mockTonerRepo.Setup(mTR => mTR.GetToner(stockLocationToners[0].tonerId)).Returns(toner);



            var sut = new StockLocationHandler(mockStockLocationRepo.Object, mockCustomerRepo.Object,
                mockUserRepo.Object, mockTonerRepo.Object, mockStockLocationTonerRepo.Object);

            //action
            var res = (JsonResult)sut.GetTonerStockLocationForGrid(stockLocationId, username);

            //assert
            res.Data.Should().BeEquivalentTo(expectedGridData);
            res.JsonRequestBehavior.Should().BeEquivalentTo(JsonRequestBehavior.AllowGet);

        }
    }
}