using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class BoatDetailViewModel : ObservableObject
    {

        private readonly BoatRepository _boatRepo = new BoatRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();

        public IEnumerable<Damage> Top3RecentDamages => Damages.Take(3);

        public ObservableCollection<Damage> Damages { get; } = new();

        public IRelayCommand DeleteBoatCommand { get; }
        public IRelayCommand EditBoatCommand { get; }

        [ObservableProperty]
        private string boatDisplayText;

        [ObservableProperty]
        private string steeringModeText;

        [ObservableProperty]
        private string boatLevelText;

        [ObservableProperty]
        private string seatsAmount;

        [ObservableProperty]
        private string boatStatusText;

        public BoatDetailViewModel(int boatId)
        {
            LoadBoatDetails(boatId);
            LoadDamagesByBoatId(boatId);
            DeleteBoatCommand = new RelayCommand(OnDeleteBoat);
            EditBoatCommand = new RelayCommand(OnEditBoat);

        }

        private void OnDeleteBoat()
        {
            // Implement boat deletion logic here
            Debug.WriteLine("DeleteBoat command executed.");
        }

        private void OnEditBoat()
        {
            // Implement boat editing logic here
            Debug.WriteLine("EditBoat command executed.");
        }

        private void LoadBoatDetails(int boatId)
        {
            var boat = _boatRepo.GetById(boatId);

            if (boat == null)
            {
                Debug.WriteLine("Boat could not be loaded.");
                return;
            }

            boatDisplayText = boat.Name;
            steeringModeText = boat.SteeringWheelPosition.ToString();
            boatLevelText = boat.Level.ToString();
            seatsAmount = boat.SeatsAmount.ToString();
            boatStatusText = boat.BoatStatus.ToString();

            OnPropertyChanged(nameof(boatDisplayText));
            OnPropertyChanged(nameof(steeringModeText));
            OnPropertyChanged(nameof(boatLevelText));
            OnPropertyChanged(nameof(seatsAmount));
            OnPropertyChanged(nameof(boatStatusText));
        }

        private void LoadDamagesByBoatId(int boatId)
        {
            Damages.Clear();

            var damages = _damageRepository.GetByBoatId(boatId);

            foreach (var damage in damages)
            {
                Damages.Add(damage);
            }
            OnPropertyChanged(nameof(Damages));
            OnPropertyChanged(nameof(Top3RecentDamages));
        }


    }
}
