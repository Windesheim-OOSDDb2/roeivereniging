using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IAuthService
    {
        public User? Login(string email, string password);

        public bool IsAdmin(User user);
    }
}
