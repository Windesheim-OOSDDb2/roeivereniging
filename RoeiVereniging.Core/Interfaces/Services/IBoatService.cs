using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IBoatService
    {
        public Boat? Get(string name);
        public Boat? Get(int id);
        public Boat? Get(int amount, bool steeringwheelposition, string difficulty, BoatType type);
        public Boat? GetById(int id);
        public List<Boat> GetAll();
    }
}
