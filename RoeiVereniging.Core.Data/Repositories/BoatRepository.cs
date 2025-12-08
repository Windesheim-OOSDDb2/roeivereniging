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
                    type INTEGER NOT NULL,
                    level INTEGER NOT NULL,
                    status INTEGER NOT NULL,
                    seats_amount INTEGER NOT NULL,
                    SteeringwheelPosition BOOLEAN NOT NULL
                );
            ");

            InsertMultipleWithTransaction(new List<string> {
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(1,'Zwarte Parel','wedstrijd',1,'available',4, 1)",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(2,'Zwarte Parel 2','training',1,'available',2, 0)",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(3,'Zwarte Parel 3','recreatie',1,'available',1, 1)"
            });
            LoadBoats();
        }

        private void LoadBoats()
        {
            boatList.Clear();

            string sql = "SELECT * FROM boat";

            OpenConnection();
            using var command = new SqliteCommand(sql, Connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string type = reader.GetString(2);
                int level = reader.GetInt32(3);
                string status = reader.GetString(4);
                int seats = reader.GetInt32(5);
                bool steering = reader.GetBoolean(6);

                var boat = new Boat(
                    id,
                    name,
                    seats,
                    steering,
                    level,
                    BoatStatus.Working,
                    BoatType.Roeiboot
                );

                boatList.Add(boat);
            }

            CloseConnection();
        }

        public Boat? Get(string name)
        {
            Boat? boat = boatList.FirstOrDefault(b => b.Name.Equals(name));
            return boat;
        }

        public Boat? Get(int id)
        {
            Boat? boat = boatList.FirstOrDefault(b => b.BoatId.Equals(id));
            return boat;
        }

        public Boat? Get(int amount, bool steeringwheelposition, string difficulty, BoatType type)
        {
            if (!int.TryParse(difficulty, out int minLevel))
            {
                return boatList.FirstOrDefault();
            }

            var boat = boatList.FirstOrDefault(b =>
                b.SeatsAmount == amount &&
                b.SteeringWheelPosition == steeringwheelposition &&
                b.Level == minLevel &&
                b.BoatType == type);

            return boat ?? boatList.FirstOrDefault();
        }

        public List<Boat> GetAll()
        {
            return boatList;
        }

        public void GetAllFromDB()
        {
            boatList.Clear();
            OpenConnection();
            using var command = Connection.CreateCommand();
            command.CommandText = "SELECT boat_id, name, seats_amount, SteeringwheelPosition, level, status, type FROM boat";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                int seatsAmount = reader.GetInt32(2);
                bool steeringWheelPosition = reader.GetBoolean(3);
                int level = reader.GetInt32(4);
                BoatStatus boatStatus = (BoatStatus)reader.GetInt32(5);
                BoatType boatType = (BoatType)reader.GetInt32(6);

                var boat = new Boat(id, name, seatsAmount, steeringWheelPosition, level, boatStatus, boatType);
                boatList.Add(boat);
            }
            CloseConnection();
        }
    }
}
