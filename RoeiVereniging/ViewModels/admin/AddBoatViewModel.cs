using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Services;
using RoeiVereniging.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace RoeiVereniging.ViewModels
{
    public partial class AddBoatViewModel : BaseViewModel
    {
        private readonly IBoatService _boatService;

        public string BoatTypeDisplay => BoatType.GetEnumDescription();

        public ObservableCollection<BoatType> BoatTypes { get; } =
    new ObservableCollection<BoatType>(
        Enum.GetValues(typeof(BoatType)).Cast<BoatType>());


        [ObservableProperty] public ObservableCollection<BoatLevel> levels = new ObservableCollection<BoatLevel>() { BoatLevel.Beginner, BoatLevel.Gemiddeld, BoatLevel.Gevorderd, BoatLevel.Expert  };


        [ObservableProperty] private string name;
        [ObservableProperty] private bool steeringWheelPosition;
        [ObservableProperty] private BoatType boatType;
        [ObservableProperty] private bool isVisible = true;
        [ObservableProperty] private bool canHaveSteering;
        [ObservableProperty] private bool steeringModeEnabled;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool addProductEnabled;
        private int seatsAmount;
        private BoatLevel boatlevel;
        private BoatStatus boatStatus;

        public AddBoatViewModel(IBoatService boatService)
        {
            _boatService = boatService;
            addProductEnabled = true;
        }

        partial void OnBoatTypeChanged(BoatType value)
        {
            OnPropertyChanged(nameof(BoatTypeDisplay));
        }

        [RelayCommand]
        public void AddBoat()
        {
            if (!string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z0-9À-ž ]+$"))
            {
                switch (BoatType)
                {
                    case BoatType.onex:
                        seatsAmount = 1;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Expert;
                        break;

                    case BoatType.twox:
                        seatsAmount = 2;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Gemiddeld;
                        break;

                    case BoatType.fourxmin:
                        seatsAmount = 4;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Gevorderd;
                        break;

                    case BoatType.fourxplus:
                        seatsAmount = 4;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Gemiddeld;
                        break;

                    case BoatType.Conex:
                        seatsAmount = 1;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.Ctwox:
                        seatsAmount = 2;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.Ctwoxplus:
                        seatsAmount = 2;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.Cfourxplus:
                        seatsAmount = 4;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.twomin:
                        seatsAmount = 2;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Gevorderd;
                        break;

                    case BoatType.twoplus:
                        seatsAmount = 2;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Gemiddeld;
                        break;

                    case BoatType.fourmin:
                        seatsAmount = 4;
                        steeringWheelPosition = false;
                        boatlevel = BoatLevel.Gevorderd;
                        break;

                    case BoatType.fourplus:
                        seatsAmount = 4;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Gemiddeld;
                        break;

                    case BoatType.eightplus:
                        seatsAmount = 8;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.Ctwoplus:
                        seatsAmount = 2;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    case BoatType.Cfourplus:
                        seatsAmount = 4;
                        steeringWheelPosition = true;
                        boatlevel = BoatLevel.Beginner;
                        break;

                    default:
                        
                        break;
                }

                Boat boat = new Boat(1, Name,seatsAmount, steeringWheelPosition,boatlevel, boatStatus, BoatType);
                _boatService.Add(boat);
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = "geen bijzondere karakters";
            }

        }


    }
}


