using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Handlers
{
    
    public class RegistrationHandler : IRegistrationHandler
    {
        private readonly IUserRepo _userRepo;

        public RegistrationHandler(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public HttpStatusCodeResult RegisterUser(UserRegisterModel userToRegister)
        {
            var userValid = ValidateUser(userToRegister);
            if (!userValid)
            {
                return new HttpStatusCodeResult(422);
            }
            var userDoesNotExist = false;
            userDoesNotExist = _userRepo.GetUsers(userToRegister.UserId).Count == 0;
            if (!userDoesNotExist)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            {
                var user = new User
                {
                    userLogin = userToRegister.UserId,
                    hashedPassword = Sodium.PasswordHash.ArgonHashString(userToRegister.Password)
                };
                _userRepo.AddUser(user);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        private bool ValidateUser(UserRegisterModel userToRegister)
        {
            //verification
            const string passwordRegEx = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,64}$";
            var passwordsMatch = (userToRegister.ConfirmPassword == userToRegister.Password);
            var passwordsMeetRequirements = (Regex.IsMatch(userToRegister.Password, passwordRegEx));
            var userNameMeetsRequirements = (!userToRegister.UserId.IsNullOrWhiteSpace());
            
            return (passwordsMatch && passwordsMeetRequirements && userNameMeetsRequirements );
        }
    }
}