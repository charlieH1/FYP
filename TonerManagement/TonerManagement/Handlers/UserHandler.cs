using System.Collections.Generic;
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
    }
}