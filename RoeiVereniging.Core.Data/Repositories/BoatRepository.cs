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
                    SteeringwheelPosition BOOL NOT NULL
                );
            ");

            InsertMultipleWithTransaction(new List<string> {
                $@"INSERT OR REPLACE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(1,'Zwarte Parel',{(int)BoatType.Roeiboot},1,{(int)BoatStatus.Working},4, true)",
                $@"INSERT OR REPLACE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(2,'Blauwe Dolfijn',{(int)BoatType.Kano},2,{(int)BoatStatus.Working},2, true)",
                $@"INSERT OR REPLACE INTO boat (boat_id, name, type, level, status, seats_amount, SteeringwheelPosition) VALUES(3,'Snelle Tonijn',{(int)BoatType.Kano},1,{(int)BoatStatus.Working},1, true)"
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