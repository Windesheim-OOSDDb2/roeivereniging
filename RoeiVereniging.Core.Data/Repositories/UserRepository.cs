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

            // seed 1 user (use OR IGNORE so repeated runs won't duplicate)
            InsertMultipleWithTransaction(new List<string> {
                @"INSERT OR IGNORE INTO user (user_id, name, email, password, role, level) VALUES(1,'Test user','test@test.nl','1234','member',1)"
            });
        }

        public User? GetById(int id)
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

        // Add authentication method here (and create if needed)
    }
}
