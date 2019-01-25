using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IRegistrationHandler
    {
        HttpStatusCodeResult RegisterUser(UserRegisterModel userToRegister);
    }
}