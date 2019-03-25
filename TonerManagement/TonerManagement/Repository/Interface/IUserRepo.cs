using System.Collections.Generic;
using System.Data.Entity;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface IUserRepo
    {
        void AddUser(User user);
        DbSet<User> GetAllUsers();
        List<User> GetUsers(string userName);
        bool UpdateUser(User user);
        User GetUser(int userId);
    }
}