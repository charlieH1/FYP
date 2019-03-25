using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepo _userRepo;

        public UserHandler(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public List<User> GetUsers(string userName)
        {
            return _userRepo.GetUsers(userName);
        }

        public HttpStatusCodeResult UpdateUser(UserUpdateModel updateUser)
        {
            var oldUser = _userRepo.GetUser(updateUser.UserId);
            var user = new User();
            if (oldUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound ,"The user was not found");
            }

            user.userId = oldUser.userId;

            if (!Sodium.PasswordHash.ArgonHashStringVerify(oldUser.hashedPassword, updateUser.CurrentPassword))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized,"The password or userId did not match");
            }

            if (string.IsNullOrEmpty(updateUser.UserName))
            {
                return new HttpStatusCodeResult(422,"The user name can not be null or empty");
            }

            user.userLogin = updateUser.UserName;

            if (!string.IsNullOrEmpty(updateUser.NewPassword))
            {
                if (updateUser.NewPassword!=updateUser.ConfirmNewPassword)
                {
                    return new HttpStatusCodeResult(422,"The new password and confirmation do not match");
                }
                const string passwordRegEx = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,64}$";
                if (!Regex.IsMatch(updateUser.NewPassword, passwordRegEx))
                {
                    return new HttpStatusCodeResult(422, "The new password does not match the security requirements");
                }
                user.hashedPassword = Sodium.PasswordHash.ArgonHashString(updateUser.NewPassword);
                
            }
            else
            {
                user.hashedPassword = oldUser.hashedPassword;
            }

            var result = _userRepo.UpdateUser(user);
            return result == true ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(422, "Failed to update DB");
            


        }
    }
}