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
                    firstName TEXT NOT NULL,
                    lastName TEXT NOT NULL,
                    email TEXT NOT NULL,
                    password TEXT NOT NULL,
                    role INTEGER NOT NULL,
                    level INTEGER NOT NULL,
                    dateOfBirth DATETIME NOT NULL,
                    registrationDate TEXT NOT NULL,
                    lastActiveDate TEXT NOT NULL
                );
            ");

            string hashedPassword = PasswordHelper.HashPassword("test");
            string now = DateTime.UtcNow.ToString("o");
            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT OR IGNORE INTO user (user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) VALUES(1,'Test', 'User','test@test.nl', '{hashedPassword}', {(int)Role.User}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{now}', '{now}')",
                $@"INSERT OR IGNORE INTO user (user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) VALUES(2,'Test', 'User 2','admin@test.nl', '{hashedPassword}', {(int)Role.Admin}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{now}', '{now}')"
            });
        }

        public User? Get(string email)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate FROM user WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            User? user = null;

            if (reader.Read())
            {
                var reg = DateTime.Parse(reader.GetString(9));
                var last = reader.IsDBNull(10) ? reg : DateTime.Parse(reader.GetString(10));
                user = new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   reader.GetString(4),
                                   reader.GetString(5),
                                   (Role)reader.GetInt32(6),
                                   (BoatLevel)reader.GetInt32(7),
                                   DateOnly.FromDateTime(reader.GetDateTime(8),
                                   reg,
                                   last)
                               );
            }
            CloseConnection();

            return user;
        }

        public User? Get(int id)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate FROM user WHERE user_id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            User? user = null;
            if (reader.Read())
            {
                var reg = DateTime.Parse(reader.GetString(9));
                var last = reader.IsDBNull(10) ? reg : DateTime.Parse(reader.GetString(10));
                user = new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   reader.GetString(4),
                                   reader.GetString(5),
                                   (Role)reader.GetInt32(6),
                                   (BoatLevel)reader.GetInt32(7),
                                   DateOnly.FromDateTime(reader.GetDateTime(8),
                                   reg,
                                   last
                               );
            }
            CloseConnection();
            return user;
        }

        public List<User> GetAll()
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate FROM user";
            using var reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                var reg = DateTime.Parse(reader.GetString(9));
                var last = reader.IsDBNull(10) ? reg : DateTime.Parse(reader.GetString(10));
                users.Add(new User(
                                   reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2),
                                   reader.GetString(3),
                                   reader.GetString(4),
                                   reader.GetString(5),
                                   (Role)reader.GetInt32(6),
                                   (BoatLevel)reader.GetInt32(7),
                                   DateOnly.FromDateTime(reader.GetDateTime(8),
                                   reg,
                                   last)
                               ));
            }
            CloseConnection();
            return users;
        }
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


        public void UpdateLastActive(int userId)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET last_active_date = @ts WHERE user_id = @id";
            cmd.Parameters.AddWithValue("@ts", DateTime.UtcNow.ToString("o"));
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
}
