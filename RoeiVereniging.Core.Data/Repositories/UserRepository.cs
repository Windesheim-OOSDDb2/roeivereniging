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
            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT OR IGNORE INTO user (user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) VALUES(1,'Test', 'User','test@test.nl', '{hashedPassword}', {(int)Role.User}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')",
                $@"INSERT OR IGNORE INTO user (user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) VALUES(2,'Michaellean', 'User 2','admin@test.nl', '{hashedPassword}', {(int)Role.Admin}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss} ', ' {DateTime.Now:yyyy-MM-dd HH:mm:ss}')",
                $@"INSERT OR IGNORE INTO user (user_id, firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) VALUES(3,'Materiaal', 'Commisaris','mats@com.nl', '{hashedPassword}', {(int)Role.Materiallcommissaris}, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss} ', ' {DateTime.Now:yyyy-MM-dd HH:mm:ss}')"
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
                user = new User(
                    reader.GetInt32(0), 
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetString(3), 
                    reader.GetString(4), 
                    (Role)reader.GetInt32(5),
                    (BoatLevel)reader.GetInt32(6), 
                    DateOnly.FromDateTime(reader.GetDateTime(7)), 
                    reader.GetDateTime(8),
                    reader.GetDateTime(9)
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
                var reg = DateTime.Parse(reader.GetString(8));
                var last = reader.IsDBNull(9) ? reg : DateTime.Parse(reader.GetString(9));
                user = new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    (Role)reader.GetInt32(5),
                    (BoatLevel)reader.GetInt32(6),
                    DateOnly.FromDateTime(reader.GetDateTime(7)),
                    reader.GetDateTime(8),
                    reader.GetDateTime(9)
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
                users.Add(new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    (Role)reader.GetInt32(5),
                    (BoatLevel)reader.GetInt32(6),
                    DateOnly.FromDateTime(reader.GetDateTime(7)),
                    reader.GetDateTime(8),
                    reader.GetDateTime(9)
                ));
            }
            CloseConnection();
            return users;
        }
           
        public User? UpdateActiveStatus(User user)
        {
            OpenConnection();
            var now = DateTime.UtcNow.ToString("o");
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET lastActiveDate = @lastActiveDate WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("@lastActiveDate", now);
            cmd.Parameters.AddWithValue("@userId", user.UserId);

            cmd.ExecuteNonQuery();
            CloseConnection();

            //update user object
            user.LastActiveDate = DateTime.Parse(now);
            return user;
        }
                 

        public User? Set(User newuser)
        {
            OpenConnection();
            var now = DateTime.UtcNow.ToString("o");
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO user (firstName, lastName, email, password, role, level, dateOfBirth, registrationDate, lastActiveDate) 
                VALUES (@firstName, @lastName, @EmailAddress, @password, @role, @level, @dateOfBirth, @registrationDate, @lastActiveDate);
                SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("@firstName", newuser.FirstName);
            cmd.Parameters.AddWithValue("@lastName", newuser.LastName);
            cmd.Parameters.AddWithValue("@EmailAddress", newuser.EmailAddress);
            cmd.Parameters.AddWithValue("@password", newuser.Password);
            cmd.Parameters.AddWithValue("@role", (int)newuser.Role);
            cmd.Parameters.AddWithValue("@level", (int)newuser.Level);
            cmd.Parameters.AddWithValue("@dateOfBirth", newuser.DateOfBirth.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@registrationDate", now);
            cmd.Parameters.AddWithValue("@lastActiveDate", now);
            var newId = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConnection();

            newuser.RegistrationDate = DateTime.Parse(now);
            newuser.LastActiveDate = newuser.RegistrationDate;

            return new User(newId, newuser.FirstName, newuser.LastName, newuser.EmailAddress, newuser.Password, newuser.Role, newuser.Level, newuser.DateOfBirth, newuser.RegistrationDate, newuser.LastActiveDate);
        }
 
        public void UpdateLastActive(int userId)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET lastActiveDate = @ts WHERE user_id = @id";
            cmd.Parameters.AddWithValue("@ts", DateTime.UtcNow.ToString("o"));
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public User Update(User updatedUser)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE user 
                SET firstName = @firstName, 
                    lastName = @lastName, 
                    email = @EmailAddress, 
                    password = @password, 
                    role = @role, 
                    level = @level, 
                    dateOfBirth = @dateOfBirth
                WHERE user_id = @userId;";
            cmd.Parameters.AddWithValue("@firstName", updatedUser.FirstName);
            cmd.Parameters.AddWithValue("@lastName", updatedUser.LastName);
            cmd.Parameters.AddWithValue("@EmailAddress", updatedUser.EmailAddress);
            cmd.Parameters.AddWithValue("@password", updatedUser.Password);
            cmd.Parameters.AddWithValue("@role", (int)updatedUser.Role);
            cmd.Parameters.AddWithValue("@level", (int)updatedUser.Level);
            cmd.Parameters.AddWithValue("@dateOfBirth", updatedUser.DateOfBirth.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@userId", updatedUser.UserId);
            cmd.ExecuteNonQuery();
            CloseConnection();

            return updatedUser;
        }
    }
}
