using Microsoft.Data.Sqlite;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class BoatRepository : DatabaseConnection, IBoatRepository
    {
        private readonly List<Boat> boatList = [];

        public BoatRepository()
        {

            CreateTable(@"
                CREATE TABLE IF NOT EXISTS boat (
                    boat_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type TEXT NOT NULL,
                    level INTEGER NOT NULL,
                    status TEXT NOT NULL,
                    seats_amount INTEGER NOT NULL,
                    SteeringwheelPosition TEXT NOT NULL CHECK(SteeringwheelPosition IN ('rechts', 'links', 'midden'))
                );
            ");

            InsertMultipleWithTransaction(new List<string> {
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount) VALUES(1,'Zwarte Parel','wedstrijd',1,'available',4, 'rechts')",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount) VALUES(2,'Zwarte Parel 2','training',1,'available',2, 'links')",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount) VALUES(3,'Zwarte Parel 3','recreatie',1,'available',1, 'rechts')"
            });
        }

        public Boat? Get(string name)
        {
            Boat? boat = boatList.FirstOrDefault(b => b.Name.Equals(name));
            return boat;
        }

        public Boat? Get(int id)
        {
            Boat? boat = boatList.FirstOrDefault(b => b.Id.Equals(id));
            return boat;
        }

        public List<Boat> GetAll()
        {
            return boatList;
        }

        public Boat Add(Boat item)
        {
            int recordsAffected;
            string insertQuery = $"INSERT INTO Boats(Name, SteeringWheelPosition, MaxPassengers, MinLevel, BoatType) VALUES(@Name, @SteeringWheelPosition, @MaxPassengers, @MinLevel, BoatType) Returning RowId;";
            OpenConnection();
            using (SqliteCommand command = new(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("SteeringWheelPosition", item.SteeringWheelPosition);
                command.Parameters.AddWithValue("MaxPassengers", item.MaxPassengers);
                command.Parameters.AddWithValue("MinLevel", item.MinLevel);
                command.Parameters.AddWithValue("BoatType", item.BoatType);

                //recordsAffected = command.ExecuteNonQuery();
                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }
    }
}
