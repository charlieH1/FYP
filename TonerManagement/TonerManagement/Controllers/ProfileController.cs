﻿using System;
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
    public class ProfileController : Controller
    {
        private readonly IUserHandler _userHandler;
        public ProfileController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }
        // GET: Profile
        public ActionResult Index()
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string) Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            else
            {
                var user = _userHandler.GetUsers((string) Session["UserName"])[0];
                return View("Index",user);
            }
            
        }

        public ActionResult UpdateProfileRequest(UserUpdateModel updateUser)
        {
            if (Session["UserName"] == null || _userHandler.GetUsers((string)Session["UserName"]).Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var user = _userHandler.GetUsers((string) Session["UserName"])[0];
            if (user.userId != updateUser.UserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            var result = _userHandler.UpdateUser(updateUser);
            if (result.StatusCode == 200)
            {
                Session["UserName"] = updateUser.UserName;
            }
            return result;

        }
    }
}