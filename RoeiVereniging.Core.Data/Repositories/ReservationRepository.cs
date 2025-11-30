using Microsoft.Data.Sqlite;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class ReservationRepository : DatabaseConnection, IReservationRepository
    {
        private readonly List<Reservation> reservationList = [];

        public ReservationRepository()
        {
            CreateTable(@"
                CREATE TABLE IF NOT EXISTS Reservation (
                    reservation_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL,
                    start_time TEXT NOT NULL,
                    end_time TEXT NOT NULL,
                    created_at TEXT NOT NULL,
                    boat_id INTEGER NOT NULL
                );
            ");

            InsertMultipleWithTransaction(new List<string> {
                @"INSERT OR IGNORE INTO Reservation (reservation_id, user_id, start_time, end_time, created_at, boat_id) VALUES(1,1,'2025-01-10 10:00','2025-01-10 11:00','2025-01-01',1)",
                @"INSERT OR IGNORE INTO Reservation (reservation_id, user_id, start_time, end_time, created_at, boat_id) VALUES(2,1,'2025-01-11 14:00','2025-01-11 15:00','2025-01-01',2)",
                @"INSERT OR IGNORE INTO Reservation (reservation_id, user_id, start_time, end_time, created_at, boat_id) VALUES(3,1,'2025-01-12 18:00','2025-01-12 19:30','2025-01-01',3)"
            });
        }

        public List<Reservation> GetByUserId(int userId)
        {
            var list = new List<Reservation>();
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT reservation_id, user_id, start_time, end_time, created_at, boat_id FROM Reservation WHERE user_id = @u";
            cmd.Parameters.AddWithValue("@u", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Reservation(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    DateTime.Parse(reader.GetString(2)),
                    DateTime.Parse(reader.GetString(3)),
                    DateTime.Parse(reader.GetString(4)),
                    reader.GetInt32(5)
                ));
            }
            CloseConnection();
            return list;
        }

        public Reservation Set(Reservation reservation)
        {
            OpenConnection();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO reservation (user_id, start_time, end_time, created_at, boat_id)
                VALUES (@user, @start, @end, @created, @boat);
            ";
            cmd.Parameters.AddWithValue("@user", reservation.UserId);
            cmd.Parameters.AddWithValue("@start", reservation.StartTime.ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("@end", reservation.EndTime.ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("@created", reservation.CreatedAt.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@boat", reservation.BoatId);
            cmd.ExecuteNonQuery();
            CloseConnection();

            return reservation;
        }

        public List<Reservation> GetAll()
        {
            if (reservationList.Count > 0)
                reservationList.Clear();

            string selectQuery = "SELECT reservation_id, user_id, start_time, end_time, created_at, boat_id FROM Reservation";
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);    
                    int userId = reader.GetInt32(1);
                    DateTime startTime = DateTime.Parse(reader.GetString(2));
                    DateTime endTime = DateTime.Parse(reader.GetString(3));
                    DateTime createdAt = DateTime.Parse(reader.GetString(4));
                    int boatId = reader.GetInt32(5);
                    reservationList.Add(new Reservation(id, userId, startTime, endTime, createdAt, boatId));
                }
            }
            CloseConnection();
            return reservationList;
        }

        public Reservation? Get(int id)
        {
            return reservationList.FirstOrDefault(r => r.Id == id);
        }
    }
}
