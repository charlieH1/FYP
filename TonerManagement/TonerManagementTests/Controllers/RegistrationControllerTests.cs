using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TonerManagement.Controllers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagementTests.Controllers
{
    [TestClass()]
    public class RegistrationControllerTests
    {
        [TestMethod()]
        public void RegistrationRequestRegistrationHandlerReturns200RegistrationRequestReturns200()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "Pa$$w0rd!";
            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var mockRegistrationHandler = new Mock<IRegistrationHandler>();
            mockRegistrationHandler.Setup(m => m.RegisterUser(model))
                .Returns(new HttpStatusCodeResult(HttpStatusCode.OK));

            var sut = new RegistrationController(mockRegistrationHandler.Object);
            
            
            //action
            var res = sut.RegistrationRequest(model);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        [TestMethod()]
        public void RegistrationRequestRegistrationHandlerReturns409RegistrationRequestReturns409()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "Pa$$w0rd!";
            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var mockRegistrationHandler = new Mock<IRegistrationHandler>();
            mockRegistrationHandler.Setup(m => m.RegisterUser(model))
                .Returns(new HttpStatusCodeResult(HttpStatusCode.Conflict));

            var sut = new RegistrationController(mockRegistrationHandler.Object);


            //action
            var res = sut.RegistrationRequest(model);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(HttpStatusCode.Conflict));
        }
        [TestMethod()]
        public void RegistrationRequestRegistrationHandlerReturns422RegistrationRequestReturns422()
        {
            //setup
            const string userName = "Test";
            const string password = "Pa$$w0rd!";
            const string confirmPassword = "DifferentPa$$w0rd!";
            var model = new UserRegisterModel
            {
                UserId = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var mockRegistrationHandler = new Mock<IRegistrationHandler>();
            mockRegistrationHandler.Setup(m => m.RegisterUser(model))
                .Returns(new HttpStatusCodeResult(422));

            var sut = new RegistrationController(mockRegistrationHandler.Object);


            //action
            var res = sut.RegistrationRequest(model);

            //assert
            res.Should().BeEquivalentTo(new HttpStatusCodeResult(422));
        }

        [TestMethod]
        public void RegisterReturnsRegisterView()
        {
            //setup
            var mockRegistrationHandler = new Mock<IRegistrationHandler>();
            var sut = new RegistrationController(mockRegistrationHandler.Object);

            //action
            var res =(ViewResult) sut.Register();

            //assert
            res.ViewName.Should().Be("~/Views/Registration/Register.cshtml");
        }
    }
}