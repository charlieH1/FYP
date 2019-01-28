using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        private readonly IUserRepo _userRepo;

        public LoginHandler(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public HttpStatusCodeResult LoginRequest(LoginModel login)
        {
            var userList = _userRepo.GetUsers(login.UserName);
            if (userList.Count==0)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            var user = userList[0];
            return !Sodium.PasswordHash.ArgonHashStringVerify(user.hashedPassword, login.Password) ? new HttpStatusCodeResult(HttpStatusCode.Unauthorized) : new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}