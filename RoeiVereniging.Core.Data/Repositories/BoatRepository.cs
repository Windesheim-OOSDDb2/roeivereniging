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
            // CREATE NEW TABLE
            CreateTable(@"
                CREATE TABLE IF NOT EXISTS boat (
                    boat_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type INTEGER NOT NULL,
                    level INTEGER NOT NULL,
                    status INTEGER NOT NULL,
                    seats_amount INTEGER NOT NULL,
                    SteeringwheelPosition BOOL NOT NULL
                );
            ");

            // INSERT DEFAULT DATA
            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition)
                   VALUES (1,'Zwarte Parel',{(int)BoatType.C},1,{(int)BoatStatus.Working},4,1)",

                $@"INSERT INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition)
                   VALUES (2,'Blauwe Dolfijn',{(int)BoatType.Scull},2,{(int)BoatStatus.Working},2,1)",

                $@"INSERT INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition)
                   VALUES (3,'Snelle Tonijn',{(int)BoatType.Boord},1,{(int)BoatStatus.Working},1,1)"
            });

            GetAllFromDB();
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

        public Boat? Get(int amount, bool steeringwheelposition, BoatLevel level, BoatType type)
        {

            var boat = boatList.FirstOrDefault(b =>
                b.SeatsAmount == amount &&
                b.SteeringWheelPosition == steeringwheelposition &&
                b.Level == level &&
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
                BoatLevel level = (BoatLevel)reader.GetInt32(4);
                BoatStatus boatStatus = (BoatStatus)reader.GetInt32(5);
                BoatType boatType = (BoatType)reader.GetInt32(6);

                var boat = new Boat(id, name, seatsAmount, steeringWheelPosition, level, boatStatus, boatType);
                boatList.Add(boat);
            }
            CloseConnection();
        }

        public Boat Add(Boat item)
        {
            int recordsAffected;
            string insertQuery = $"INSERT INTO boat(name, Steeringwheelposition, seats_amount, level, type, status) VALUES(@Name, @SteeringWheelPosition, @Seats_Amount, @Level, @Type, @Status) Returning RowId;";
            OpenConnection();
            using (SqliteCommand command = new(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("SteeringWheelPosition", item.SteeringWheelPosition);
                command.Parameters.AddWithValue("Seats_Amount", item.SeatsAmount);
                command.Parameters.AddWithValue("Level", item.Level);
                command.Parameters.AddWithValue("Type", item.BoatType);
                command.Parameters.AddWithValue("Status", item.BoatStatus);

                //recordsAffected = command.ExecuteNonQuery();
                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }
    }
}