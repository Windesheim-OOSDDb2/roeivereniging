using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? GetById(int id);
    }
}

