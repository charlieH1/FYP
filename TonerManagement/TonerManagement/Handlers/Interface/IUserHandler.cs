using System.Collections.Generic;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IUserHandler
    {
        List<User> GetUsers(string userName);
    }
}