using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User? Login(string email, string password)
        {
            User? user = _userRepo.Get(email);
            if (user == null) return null;

            if (PasswordHelper.VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public bool IsAdmin(User user)
        {
            return true;
        }
    }
}
