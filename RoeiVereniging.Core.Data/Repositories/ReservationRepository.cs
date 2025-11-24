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
            CreateTable(@"CREATE TABLE IF NOT EXISTS Reservations (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [Name] TEXT NOT NULL,
                            [PassengerCount] INTEGER NOT NULL,
                            [DateTime] TEXT NOT NULL,
                            [UserId] INTEGER NOT NULL,
                            [BoatId] INTEGER NOT NULL,
                            UNIQUE(UserId, BoatId, DateTime)
                )");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO Reservations(Name, PassengerCount, DateTime, UserId, BoatId) VALUES('uselessfield', 4, '2025-11-24 17:04:30', 1, 1)"];
            InsertMultipleWithTransaction(insertQueries);
            GetAll();
        }

        public List<Reservation> GetAll()
        {
            if (reservationList.Count > 0)
                reservationList.Clear();

            string selectQuery = "SELECT * FROM Reservations";
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int passengerCount = reader.GetInt32(2);
                    DateTime dateTime = reader.GetDateTime(3);
                    int userId = reader.GetInt32(4);
                    int boatId = reader.GetInt32(5);
                    reservationList.Add(new Reservation(id, name, passengerCount, dateTime, userId, boatId));
                }
            }
            CloseConnection();
            return reservationList;
        }

        public Reservation? Get(int id)
        {
            return reservationList.FirstOrDefault(r => r.Id == id);
        }

        public Reservation? Set(Reservation reservation)
        {
            string insertQuery = $@"INSERT INTO Reservations (Name, PassengerCount, DateTime, UserId, BoatId)
                                    VALUES ('{reservation.Name}', {reservation.PassengerCount}, '{reservation.DateTime.ToString("yyyy-MM-dd HH:mm:ss")}', {reservation.UserId}, {reservation.BoatId});
                                    SELECT last_insert_rowid();";
            OpenConnection();
            using (SqliteCommand command = new(insertQuery, Connection))
            {
                long newId = (long)command.ExecuteScalar();
                reservation.Id = (int)newId;
                reservationList.Add(reservation);
            }
            CloseConnection();
            return reservation;
        }

    }
}
