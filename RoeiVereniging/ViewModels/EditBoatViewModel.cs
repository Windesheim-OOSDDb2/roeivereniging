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
using System.Text.RegularExpressions;
using RoeiVereniging.Core.Helpers;

namespace RoeiVereniging.ViewModels
{
    public partial class EditBoatViewModel : BaseViewModel
    {
        private readonly IBoatService _boatService;

        public string BoatTypeDisplay => BoatType.GetEnumDescription();

        public ObservableCollection<BoatType> BoatTypes { get; } =
    new ObservableCollection<BoatType>(
        Enum.GetValues(typeof(BoatType)).Cast<BoatType>());

        [ObservableProperty] private Boat boat;
        [ObservableProperty] private string name;
        [ObservableProperty] private BoatType boatType;
        [ObservableProperty] private bool isVisible = true;
        [ObservableProperty] private string errorMessage;

        private bool steeringWheelPosition;
        private int seatsAmount;
        private BoatLevel boatlevel;
        private BoatStatus boatStatus;

        public EditBoatViewModel(IBoatService boatService)
        {
            _boatService = boatService;
        }

        partial void OnBoatTypeChanged(BoatType value)
        {
            OnPropertyChanged(nameof(BoatTypeDisplay));
        }

        public void OnAppearing()
        {
            Boat = _boatService.Get(1);
            Name = Boat.Name;
            BoatType = Boat.Type;
        }

        [RelayCommand]
        public void EditBoat()
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

                Boat.Name = Name;
                Boat.Type = BoatType;
                Boat.SeatsAmount = seatsAmount;
                Boat.SteeringWheelPosition = steeringWheelPosition;
                Boat.Level = boatlevel;

                _boatService.Update(Boat);
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = "geen bijzondere karakters";
            }

        }
    }
}
