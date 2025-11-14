using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Client? Login(string email, string password);
    }
}
