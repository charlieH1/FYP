using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly TonerManagementEntities _db;
        public UserRepo(DbContext db)
        {
            _db = (TonerManagementEntities) db;
        }

        public DbSet<User> GetAllUsers()
        {
            return _db.Users;
        }

        public List<User> GetUsers(string userName)
        {
            return _db.Users.Where(a => a.userLogin == userName).ToList();
        }

        public void AddUser(User user)
        {
            var newUser = _db.Users.Create();
            newUser.userLogin = user.userLogin;
            newUser.hashedPassword = user.hashedPassword;
            _db.Users.Add(newUser);
            _db.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var oldUser = _db.Users.Single(a => a.userId == user.userId);
            oldUser.userLogin = user.userLogin;
            oldUser.hashedPassword = user.hashedPassword;
            _db.SaveChanges();
        }
    }
}