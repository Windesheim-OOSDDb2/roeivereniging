using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? Get(string username);
        public User? Get(int id);
        public List<User> GetAll();
    }
}
