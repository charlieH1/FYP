using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;

namespace TonerManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginHandler _loginHandler;

        public LoginController(ILoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        // GET: Login
        public ActionResult Login()
        {
            return View("Login");
        }

        public HttpStatusCodeResult LoginRequest(LoginModel login)
        {
            var httpStatusCodeResult = _loginHandler.LoginRequest(login);
            if (httpStatusCodeResult.StatusCode == 200)
            {
                Session["UserName"] = login.UserName;
            }

            return httpStatusCodeResult;
        }
    }
}