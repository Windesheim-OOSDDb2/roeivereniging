using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IDamageRepository
    {
        public List<Damage> GetByBoatId(int boatId);

        public void Add(Damage damage);

        public List<Damage> GetAll();
    }
}
