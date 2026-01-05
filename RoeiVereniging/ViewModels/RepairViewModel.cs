using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public class RepairViewModel : BaseViewModel
    {
        private readonly IBoatRepository _boatRepository = new BoatRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();
    }
}
