using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Repositories;

namespace RoeiVereniging.ViewModels
{
    public partial class UserDetailViewModel : ObservableObject
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly IReservationRepository _reservationRepository = new ReservationRepository();





    }
}
