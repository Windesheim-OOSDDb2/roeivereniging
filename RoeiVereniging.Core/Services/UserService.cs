using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserService _userService;
        public User? Get(string email)
        {
            return _userService.Get(email);
        }

        public User? Get(int id)
        {
            return _userService.Get(id);
        }

        public List<User> GetAll()
        {
            return _userService.GetAll();
        }
    }
}
