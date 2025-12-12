using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? Get(int id);
        public User? Get(string email);
        public List<User> GetAll();
    }
}

