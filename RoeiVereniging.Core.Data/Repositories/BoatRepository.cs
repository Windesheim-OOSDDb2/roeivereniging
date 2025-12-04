using Microsoft.Data.Sqlite;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class BoatRepository : DatabaseConnection, IBoatRepository
    {
        private readonly List<Boat> boatList;

        public BoatRepository()
        {
            boatList = [
                new Boat(1, "Zwarte Parel", 4, false, BoatLevel.Basis, BoatStatus.Working, BoatType.C),
                new Boat(2, "Blauwe Dolfijn", 2, true, BoatLevel.Gevorderd, BoatStatus.Fixing, BoatType.C),
                ];
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
