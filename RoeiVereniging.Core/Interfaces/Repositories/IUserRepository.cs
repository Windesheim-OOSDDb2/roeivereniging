using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? Get(string email);
        public User? Get(int id);
        public List<User> GetAll();
        public User? UpdateActiveStatus(User user);
        public User? Set(User newuser);
    }
}

