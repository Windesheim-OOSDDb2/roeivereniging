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
               $@"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(1,'Zwarte Parel',{(int)BoatType.onex},{(int)BoatLevel.Beginner},{(int)BoatStatus.Working},4, true)",
               $@"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(2,'Blauwe Dolfijn',{(int)BoatType.twox},{(int)BoatLevel.Expert},{(int)BoatStatus.Working},2, true)",
               $@"INSERT OR IGNORE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(3,'Snelle Tonijn',{(int)BoatType.twox},{(int)BoatLevel.Beginner},{(int)BoatStatus.Working},1, true)"
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
                int type = reader.GetInt32(2);
                int level = reader.GetInt32(3);
                int status = reader.GetInt32(4);
                int seats = reader.GetInt32(5);
                bool steering = reader.GetBoolean(6);

                var boat = new Boat(
                    id,
                    name,
                    seats,
                    steering,
                    (BoatLevel)level,
                    (BoatStatus)status,
                    (BoatType)type
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
            if (!Enum.TryParse<BoatLevel>(difficulty, true, out var level))
            {
                return boatList.FirstOrDefault();
            }

            var boat = boatList.FirstOrDefault(b =>
                b.SeatsAmount == amount &&
                b.SteeringWheelPosition == steeringwheelposition &&
                b.Level == minLevel &&
                b.Type == type);

            return boat ?? boatList.FirstOrDefault();
        }

        public List<Boat> GetAll()
        {
            return boatList;
        }

        public Boat Add(Boat item)
        {
            string insertQuery = $"INSERT INTO boat(name, Steeringwheelposition, seats_amount, level, type, status) VALUES(@Name, @SteeringWheelPosition, @Seats_Amount, @Level, @Type, @Status) Returning RowId;";
            OpenConnection();
            using (SqliteCommand command = new(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("SteeringWheelPosition", item.SteeringWheelPosition);
                command.Parameters.AddWithValue("Seats_Amount", item.SeatsAmount);
                command.Parameters.AddWithValue("Level", item.Level);
                command.Parameters.AddWithValue("Type", item.Type);
                command.Parameters.AddWithValue("Status", item.BoatStatus);

                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }
    }
}