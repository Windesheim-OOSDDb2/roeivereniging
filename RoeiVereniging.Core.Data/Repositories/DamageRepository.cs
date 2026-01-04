using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class DamageRepository : DatabaseConnection
    {
        public DamageRepository()
        {
            CreateTable(@"
                CREATE TABLE IF NOT EXISTS Damage (
                    damage_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    reservation_id INTEGER NOT NULL,
                    boat_id INTEGER NOT NULL,
                    user_id INTEGER NOT NULL,
                    description TEXT NOT NULL,
                    reported_at TEXT NOT NULL,
                    severity INTEGER NOT NULL
                );  
            ");
        }

        public void Add(Damage damage)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Damage (reservation_id, boat_id, user_id, description, reported_at, severity)
                VALUES (@reservation_id, @boatId, @user_id, @description, @reportedAt, @severity);    
            ";

            cmd.Parameters.AddWithValue("@reservation_id", damage.ReservationId);
            cmd.Parameters.AddWithValue("@boatId", damage.BoatId);
            cmd.Parameters.AddWithValue("@user_id", damage.UserId);
            cmd.Parameters.AddWithValue("@description", damage.Description);
            cmd.Parameters.AddWithValue("@reportedAt", damage.ReportedAt.ToString("o"));
            cmd.Parameters.AddWithValue("@severity", (int)damage.Severity);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public List<Damage> GetByBoatId(int boatId)
        {
            var list = new List<Damage>();
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT damage_id, reservation_id, boat_id, user_id, description, reported_at, severity FROM Damage WHERE boat_id = @boatId";
            cmd.Parameters.AddWithValue("@boatId", boatId);
            using var reader = cmd.ExecuteReader(); 
            while (reader.Read())
            {
                list.Add(new Damage(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetString(4),
                    DateTime.Parse(reader.GetString(5)),
                    (EnumDamageSeverity)reader.GetInt32(6)
                ));
            }
            CloseConnection();
            return list;
        }

        public List<Damage> GetByUserId(int userId)
        {
            var list = new List<Damage>();
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"SELECT damage_id, reservation_id, boat_id, user_id, description, reported_at, severity FROM Damage WHERE user_id = @userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Damage(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetString(4),
                    DateTime.Parse(reader.GetString(5)),
                    (EnumDamageSeverity)reader.GetInt32(6)
                ));
            }

            CloseConnection();
            return list;
        }

    }
}
