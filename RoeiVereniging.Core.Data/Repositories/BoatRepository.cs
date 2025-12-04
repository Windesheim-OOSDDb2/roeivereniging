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
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(1,'Zwarte Parel','wedstrijd',1,'available',4, 'rechts')",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(2,'Zwarte Parel 2','training',1,'available',2, 'links')",
                @"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(3,'Zwarte Parel 3','recreatie',1,'available',1, 'rechts')"
            });
            LoadBoats();
        }

        private void LoadBoats()
        {
            // clear inmemory list (no duplicates)
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
                string steering = reader.GetString(6);

                // Create Boat model based on values in db
                var boat = new Boat(
                    id,
                    name,
                    seats,
                    steering,
                    level,
                    BoatStatus.Working,
                    BoatType.Roeiboot
                );

                // add to cache
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

        public List<Boat> GetAll()
        {
            return boatList;
        }
    }
}
