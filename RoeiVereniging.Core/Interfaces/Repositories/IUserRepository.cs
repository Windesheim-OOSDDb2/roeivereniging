using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? GetById(int id);
        public User? Get(string email);
        public User? Get(int id);
        public List<User> GetAll();
    }
}

