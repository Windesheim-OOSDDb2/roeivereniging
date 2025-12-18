using Microsoft.Data.Sqlite;
using RoeiVereniging.Core.Data;
using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Repositories
{
    public class UserRepository : DatabaseConnection, IUserRepository
    {
        private readonly List<User> UserList = [];

        public UserRepository()
        {
            // For login -> table is like ERD
            CreateTable(@"
                CREATE TABLE IF NOT EXISTS user (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    email TEXT NOT NULL,
                    password TEXT NOT NULL,
                    role TEXT NOT NULL,
                    level INTEGER NOT NULL
                );
            ");

            string hashedPassword = PasswordHelper.HashPassword("test");
            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT OR IGNORE INTO user (user_id, name, email, password, role, level) VALUES(1,'Test user','test@test.nl', '{hashedPassword}', 'member', 1)"
            });

        }

        public User? Get(string email)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password FROM user WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            User? user = null;

            if (reader.Read())
            {
                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
            CloseConnection();

            return user;
        }

        public User? Get(int id)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password FROM user WHERE user_id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            User? user = null;
            if (reader.Read())
                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            CloseConnection();
            return user;
        }

        public List<User> GetAll()
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password FROM user";
            using var reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            }
            CloseConnection();
            return users;
        }
    }
}
