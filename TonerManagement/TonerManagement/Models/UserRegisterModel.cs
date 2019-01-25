using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class UserRegisterModel
    {
        public string UserId { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}