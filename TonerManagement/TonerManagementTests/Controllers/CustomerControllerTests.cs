using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Controllers;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagementTests.TestHelpers;

namespace TonerManagementTests.Controllers
{
    [TestClass()]
    public class CustomerControllerTests
    {
        

        [TestMethod()]
        public void GetCustomersForUserWithValidUserAndCustomerHandlerReturnsCustomersAsJson()
        {
            //setup
            const string userName = "Test";
            var customers = new List<Customer>
            {
                new Customer()
                {
                    customerAddress = "Test address 1",
                    customerContactNumber = "123456789",
                    customerID = 1,
                    customerName = "Test Customer 1"
                },
                new Customer()
                {
                    customerAddress = "Test address 2",
                    customerContactNumber = "1234567892",
                    customerID = 2,
                    customerName = "Test Customer 2"
                }
            };
            var json = new JsonResult()
            {
                Data = customers,
                ContentEncoding = null,
                ContentType = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            mockCustomerHandler.Setup(mCH => mCH.GetCustomersForUser(userName)).Returns(json);

            var mockSession = new MockSessionStateBase {["UserName"] = userName};
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(m => m.HttpContext.Session).Returns(mockSession);
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object,mockUserHandler.Object) {ControllerContext = mockContext.Object};
            
            //action
            var res = (JsonResult)sut.GetCustomersForUser();

            //assert
            res.Data.Should().BeEquivalentTo(customers.AsEnumerable());
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        [TestMethod()]
        public void GetCustomersForNullSessionUserReturnsUnauthorized()
        {
            //setup
            const string userName = null;
           
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            
            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(m => m.HttpContext.Session).Returns(mockSession);
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object,mockUserHandler.Object) { ControllerContext = mockContext.Object };

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomersForUser();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetCustomerForInvalidUserReturnsUnauthorized()
        {
            //setup
            const string userName = "Test";

            var mockCustomerHandler = new Mock<ICustomerHandler>();
            mockCustomerHandler.Setup(mCH => mCH.GetCustomersForUser(userName))
                .Returns(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));

            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(m => m.HttpContext.Session).Returns(mockSession);
            var mockUserHandler = new Mock<IUserHandler>();


            var sut = new CustomerController(mockCustomerHandler.Object,mockUserHandler.Object) { ControllerContext = mockContext.Object };

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomersForUser();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod()]
        public void GetCustomerWithValidUserButNoCustomersReturnsEmptyJson()
        {
            //setup
            const string userName = "Test";
            var customers = new List<Customer>();
            
            var json = new JsonResult()
            {
                Data = customers,
                ContentEncoding = null,
                ContentType = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            mockCustomerHandler.Setup(mCH => mCH.GetCustomersForUser(userName)).Returns(json);

            var mockSession = new MockSessionStateBase { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(m => m.HttpContext.Session).Returns(mockSession);
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object,mockUserHandler.Object) { ControllerContext = mockContext.Object };

            //action
            var res = (JsonResult)sut.GetCustomersForUser();

            //assert
            res.Data.Should().BeEquivalentTo(customers.AsEnumerable());
            res.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        [TestMethod]
        public void IndexRequestInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            var mockSession = new MockSessionStateBase(){["UserName"]=null};
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult) sut.Index();

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod]
        public void IndexRequestValidUserReturnsIndexPage()
        {
            //setup
            const string userName = "Test";
            var user = new User()
            {
                userLogin = userName,
                userId = 1,
                hashedPassword = "Junk"
            };

            var mockSession = new MockSessionStateBase() { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>() {user});

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (ViewResult)sut.Index();

            //assert
            res.ViewName.Should().Be("Index");
        }

        [TestMethod()]
        public void GetCustomerInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            const int customerId = 1;
            var mockSession = new MockSessionStateBase() { ["UserName"] = null };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomer(customerId);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod]
        public void GetCustomerValidUserReturnsCustomerHandlerResult()
        {
            //setup
            const string userName = "Test";
            const int customerId = 1;
            var user = new User()
            {
                userLogin = userName,
                userId = 1,
                hashedPassword = "Junk"
            };
            var expectedCustomerHandlerResult = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase() { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            mockCustomerHandler.Setup(mCH => mCH.GetCustomer(customerId,userName)).Returns(expectedCustomerHandlerResult);
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>() { user });

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.GetCustomer(customerId);

            //assert
            res.Should().BeEquivalentTo(expectedCustomerHandlerResult);
        }


        [TestMethod()]
        public void UpdateCustomerInvalidUserReturnsHttpStatusCodeUnauthorized()
        {
            //setup
            var request = new UpdateCustomerModel()
            {
                CustomerId = 1,
                CustomerPhoneNumber = "123456",
                CustomerPostalAddress = "Test Address"
            };
            var mockSession = new MockSessionStateBase() { ["UserName"] = null };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            var mockUserHandler = new Mock<IUserHandler>();

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        [TestMethod]
        public void UpdateCustomerValidUserReturnsCustomerHandlerResult()
        {
            //setup
            const string userName = "Test";
            var request = new UpdateCustomerModel()
                {
                    CustomerId = 1,
                    CustomerPhoneNumber = "123456",
                    CustomerPostalAddress = "Test Address"
                };
            var user = new User()
            {
                userLogin = userName,
                userId = 1,
                hashedPassword = "Junk"
            };
            var expectedCustomerHandlerResult = new HttpStatusCodeResult(HttpStatusCode.OK);
            var mockSession = new MockSessionStateBase() { ["UserName"] = userName };
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(mC => mC.HttpContext.Session).Returns(mockSession);
            var mockCustomerHandler = new Mock<ICustomerHandler>();
            mockCustomerHandler.Setup(mCH => mCH.UpdateCustomer(request, userName)).Returns(expectedCustomerHandlerResult);
            var mockUserHandler = new Mock<IUserHandler>();
            mockUserHandler.Setup(mUH => mUH.GetUsers(userName)).Returns(new List<User>() { user });

            var sut = new CustomerController(mockCustomerHandler.Object, mockUserHandler.Object)
            {
                ControllerContext = mockContext.Object
            };

            //action
            var res = (HttpStatusCodeResult)sut.UpdateCustomer(request);

            //assert
            res.Should().BeEquivalentTo(expectedCustomerHandlerResult);
        }
    }
}