using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.Controls.Internals.Profile;

namespace RoeiVereniging.ViewModels
{
    public partial class AddBoatViewModel : BaseViewModel
    {
        private readonly IBoatService _boatService;

        [ObservableProperty] public List<BoatLevel> minlevels = new List<BoatLevel>() {BoatLevel.Basis,BoatLevel.Gevorderd,BoatLevel.Expert};
        [ObservableProperty] public List<BoatType> boatTypes = new List<BoatType>() {BoatType.Boord,BoatType.Scull,BoatType.C,BoatType.Liteboat };
        [ObservableProperty] public List<int> maxpassenger = new List<int>() {1,2,3,4,8};


        [ObservableProperty] private string name;
        [ObservableProperty] private bool steeringWheelPosition;
        [ObservableProperty] private int maxPassengers;
        [ObservableProperty] private BoatLevel minLevel;
        [ObservableProperty] private BoatStatus boatStatus;
        [ObservableProperty] private BoatType boatType;
        [ObservableProperty] private bool isVisible = true;

        public AddBoatViewModel(IBoatService boatService)
        {
            _boatService = boatService;
        }

        [RelayCommand]
        public void AddBoat()
        {
            SteeringWheelPosition = true;
            //var boat = new Boat(1, name, maxPassengers, steeringWheelPosition, minLevel, boatStatus,boatType);
            //_boatService.Add(boat);

            //name = string.Empty;
            //steeringWheelPosition = false;
            //maxPassengers = 0;
            //minLevel = BoatLevel.Basis;
            //boatStatus = BoatStatus.Working;
            //boatType = BoatType.C;
        }


        [RelayCommand]
        public void Test()
        {

        }

        


    }
}
