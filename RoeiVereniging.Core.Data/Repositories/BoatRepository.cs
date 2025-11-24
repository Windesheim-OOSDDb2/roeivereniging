using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Data.Repositories
{
    public class BoatRepository : IBoatRepository
    {
        private readonly List<Boat> boatList;

        public BoatRepository()
        {
            boatList = [
                new Boat(1, "ZwarteParel", 4, false, 1, BoatStatus.Working, BoatType.Roeiboot)
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
    }
}
