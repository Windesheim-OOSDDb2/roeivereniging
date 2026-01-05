using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Services
{
    public class BoatService : IBoatService
    {
        private readonly IBoatRepository _boatRepository;
        public BoatService(IBoatRepository boatRepository)
        {
            _boatRepository = boatRepository;
        }

        public Boat? Get(string name)
        {
            return _boatRepository.Get(name);
        }

        public Boat? Get(int id)
        {
            return _boatRepository.Get(id);
        }


        public List<Boat> Get(int amount, bool steeringwheelposition, BoatLevel difficulty, BoatType type)
        {
            return _boatRepository.Get(amount, steeringwheelposition, difficulty, type);
        }

        public Boat? GetById(int id)
        {
            return _boatRepository.Get(id);
        }

        public List<Boat> GetAll()
        {
            return _boatRepository.GetAll();
        }

        public Boat Add(Boat item)
        {
            return _boatRepository.Add(item);
        }

        public Boat Update(Boat item)
        {
            return _boatRepository.Update(item);
        }
    }
}
