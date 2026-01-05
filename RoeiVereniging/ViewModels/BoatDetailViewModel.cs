using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
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

        }

        [RelayCommand]
        public async Task GoToEditBoat()
        {
            await Shell.Current.GoToAsync(nameof(EditBoatView));
        }


        private void LoadBoatDetails(int boatId)
        {
            Boat boat = _boatRepo.GetById(boatId);

            if (boat == null)
            {
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

            List<Damage> damages = _damageRepository.GetByBoatId(boatId);

            foreach (Damage damage in damages)
            {
                Damages.Add(damage);
            }
            OnPropertyChanged(nameof(Damages));
            OnPropertyChanged(nameof(Top3RecentDamages));
        }


    }
}
