using System.Collections.Generic;
using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IUserHandler
    {
        List<User> GetUsers(string userName);
        HttpStatusCodeResult UpdateUser(UserUpdateModel updateUser);
    }
}