using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IUserService
    {
        public User? Get(string email);
        public User? Get(int id);
        public List<User> GetAll();
    }
}
