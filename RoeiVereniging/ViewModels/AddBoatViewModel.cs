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


namespace RoeiVereniging.ViewModels
{
    public partial class AddBoatViewModel : BaseViewModel
    {
        private readonly IBoatService _boatService;

        [ObservableProperty] public ObservableCollection<BoatLevel> levels = new ObservableCollection<BoatLevel>() { BoatLevel.Beginner, BoatLevel.Gemiddeld, BoatLevel.Gevorderd, BoatLevel.Expert  };
        [ObservableProperty] public ObservableCollection<BoatType> boatTypes = new ObservableCollection<BoatType>() { BoatType.Boord, BoatType.Scull, BoatType.C, BoatType.Liteboat };
        [ObservableProperty] public ObservableCollection<int> maxpassenger = new ObservableCollection<int>() { 1, 2, 3, 4, 8 };


        [ObservableProperty] private string name;
        [ObservableProperty] private bool steeringWheelPosition;
        [ObservableProperty] private int maxPassengers;
        [ObservableProperty] private BoatLevel level;
        [ObservableProperty] private BoatStatus boatStatus;
        [ObservableProperty] private BoatType boatType;
        [ObservableProperty] private bool isVisible = true;
        [ObservableProperty] private bool canHaveSteering;
        [ObservableProperty] private bool steeringModeEnabled;
        [ObservableProperty] private bool levelsEnabled = true;
        [ObservableProperty] private bool maxPassengersEnabled;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool addProductEnabled;

        public AddBoatViewModel(IBoatService boatService)
        {
            _boatService = boatService;
            MaxPassengersEnabled = true;
            maxPassengers = 1;
            SteeringWheelCheck();
            addProductEnabled = true;
        }

        partial void OnBoatTypeChanged(BoatType value)
        {
            SteeringWheelCheck();
        }

        partial void OnMaxPassengersChanged(int value)
        {
            SteeringWheelCheck();
        }




        [RelayCommand]
        public void AddBoat()
        {
            if (!string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z0-9À-ž ]+$"))
            {
                var boat = new Boat(1, name, maxPassengers, steeringWheelPosition, level, boatStatus, boatType);
                _boatService.Add(boat);
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = "geen bijzondere karakters";
            }

        }


        [RelayCommand]
        public void SteeringWheelCheck()
        {
            var mode = SteeringMode.Disabled;
            var error = false;


            // Determine new collections based on BoatType
            switch (BoatType)
            {
                case BoatType.C:

                    if (MaxPassengers == 2)
                        mode = SteeringMode.Optional;
                    else if (MaxPassengers == 3 || MaxPassengers == 4)
                        mode = SteeringMode.Required;
                    else if (MaxPassengers == 8)
                        error = true;

                    break;

                case BoatType.Scull:

                    if (MaxPassengers == 2 || MaxPassengers == 3 || MaxPassengers == 4)
                        mode = SteeringMode.Optional;
                    else if (MaxPassengers == 8)
                        mode = SteeringMode.Required;
                    else if (Level == BoatLevel.Beginner)
                        error = true;

                    break;

                case BoatType.Boord:

                    if (MaxPassengers == 2 || MaxPassengers == 4)
                        mode = SteeringMode.Optional;
                    else if (MaxPassengers == 8)
                        mode = SteeringMode.Required;
                    else if(MaxPassengers == 1 || MaxPassengers== 3 ||Level == BoatLevel.Beginner || Level == BoatLevel.Gevorderd || Level == BoatLevel.Gemiddeld) 
                        error = true;
                    break;

                case BoatType.Liteboat:
                    mode = SteeringMode.Disabled;
                    if (MaxPassengers != 1)
                        error = true;

                    break;

                default:
                    LevelsEnabled = true;
                    MaxPassengersEnabled = true;
                    AddProductEnabled = true;
                    ErrorMessage = "";
                    break;
            }

            // Update steering properties
            SteeringModeEnabled = mode == SteeringMode.Optional;
            SteeringWheelPosition = mode switch
            {
                SteeringMode.Disabled => false,
                SteeringMode.Required => true,
                SteeringMode.Optional => SteeringWheelPosition,
                _ => false
            };

            AddProductEnabled = error switch
            {
                false => true,
                true => false

            };
            ErrorMessage = error switch
            {
                false => "",
                true => "Deze combinatie mag niet"

            };
        }

    }
}


