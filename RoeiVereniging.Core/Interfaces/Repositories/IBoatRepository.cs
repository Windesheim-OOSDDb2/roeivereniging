using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IBoatRepository
    {
        public Boat? Get(string name);
        public Boat? Get(int id);
        public Boat? Get(int amount, bool steeringwheelposition, string difficulty, BoatType type);
        public Boat? GetById(int id);
        public List<Boat> GetAll();
        public Boat Add(Boat item);

        public Boat Update(Boat item);
    }
}
