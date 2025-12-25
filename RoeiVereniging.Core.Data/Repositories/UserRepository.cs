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
                    role INTEGER NOT NULL,
                    level INTEGER NOT NULL,
                    dateOfBirth DATETIME NOT NULL
                );
            ");

            string hashedPassword = PasswordHelper.HashPassword("test");
            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT OR IGNORE INTO user (user_id, name, email, password, role, level, dateOfBirth) VALUES(1,'Test user','test@test.nl', '{hashedPassword}', {(int)Role.User}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')",
                $@"INSERT OR IGNORE INTO user (user_id, name, email, password, role, level, dateOfBirth) VALUES(2,'Test user','admin@test.nl', '{hashedPassword}', {(int)Role.Admin}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')",
                $@"INSERT OR IGNORE INTO user (user_id, name, email, password, role, level, dateOfBirth) VALUES(2,'Materiaal Commisaris','mats@com.nl', '{hashedPassword}', {(int)Role.Materiallcommissaris}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')"
            });
        }

        public User? Get(string email)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password, role, level, dateOfBirth FROM user WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            User? user = null;

            if (reader.Read())
            {
                user = new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   (Role)reader.GetInt32(4),
                                   (BoatLevel)reader.GetInt32(5),
                                   DateOnly.FromDateTime(reader.GetDateTime(6))
                               );
            }
            CloseConnection();

            return user;
        }

        public User? Get(int id)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password, role, level, dateOfBirth FROM user WHERE user_id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            User? user = null;
            if (reader.Read())
                user = new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   (Role)reader.GetInt32(4),
                                   (BoatLevel)reader.GetInt32(5),
                                   DateOnly.FromDateTime(reader.GetDateTime(6))
                               );
            CloseConnection();
            return user;
        }

        public List<User> GetAll()
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, name, email, password, role, level, dateOfBirth FROM user";
            using var reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   (Role)reader.GetInt32(4),
                                   (BoatLevel)reader.GetInt32(5),
                                   DateOnly.FromDateTime(reader.GetDateTime(6))
                               ));
            }
            CloseConnection();
            return users;
        }

        public User? Set(User newuser)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO user (name, email, password, role, level, dateOfBirth) 
                VALUES (@name, @EmailAddress, @password, @role, @level, @dateOfBirth);
                SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("@name", newuser.Name);
            cmd.Parameters.AddWithValue("@EmailAddress", newuser.EmailAddress);
            cmd.Parameters.AddWithValue("@password", newuser.Password);
            cmd.Parameters.AddWithValue("@role", (int)newuser.Role);
            cmd.Parameters.AddWithValue("@level", (int)newuser.Level);
            cmd.Parameters.AddWithValue("@dateOfBirth", newuser.DateOfBirth.ToDateTime(TimeOnly.MinValue));

            var newId = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConnection();

            return new User(newId, newuser.Name, newuser.EmailAddress, newuser.Password, newuser.Role, newuser.Level, newuser.DateOfBirth);
        }
    }
}
