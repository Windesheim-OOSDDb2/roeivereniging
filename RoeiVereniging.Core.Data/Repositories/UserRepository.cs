using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> UserList;

        public UserRepository()
        {
            User admin = new(3, "A.J. Kwak", "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=");
            admin.Role = Role.Admin;
            UserList = [
                new User(1, "M.J. Curie", "user1@mail.com", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08="),
                new User(2, "H.H. Hermans", "user2@mail.com", "dOk+X+wt+MA9uIniRGKDFg==.QLvy72hdG8nWj1FyL75KoKeu4DUgu5B/HAHqTD2UFLU="),
                admin
            ];
        }

        public User? Get(string email)
        {
            User? User = UserList.FirstOrDefault(c => c.EmailAddress.Equals(email));
            return User;
        }

        public User? Get(int id)
        {
            User? User = UserList.FirstOrDefault(c => c.Id == id);
            return User;
        }

        public List<User> GetAll()
        {
            return UserList;
        }
    }
}
