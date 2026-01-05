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

        [ObservableProperty]
        private Boat boat;

        [ObservableProperty]
        private string changeBoatStatusButtonText;

        public BoatDetailViewModel(int boatId)
        {
            LoadBoatDetails(boatId);
            LoadDamagesByBoatId(boatId);
        }

        [RelayCommand]
        private void ChangeBoatStatus()
        {
            if (Boat.BoatStatus != BoatStatus.Gearchiveerd)
            {
                Boat.BoatStatus = BoatStatus.Gearchiveerd;
                ChangeBoatStatusButtonText = "Herstellen";
            }
            else
            {
                Boat.BoatStatus = BoatStatus.Werkend;
                ChangeBoatStatusButtonText = "Archiveren";
            }
            _boatRepo.UpdateStatus(Boat);
            OnPropertyChanged(nameof(Boat));
        }

        [RelayCommand]
        public async Task GoToEditBoat(int boatId)
        {
            if (Boat.BoatStatus != BoatStatus.Gearchiveerd)
            {
                Boat.BoatStatus = BoatStatus.Gearchiveerd;
                ChangeBoatStatusButtonText = "Herstellen";
            }
            else
            {
                Boat.BoatStatus = BoatStatus.Werkend;
                ChangeBoatStatusButtonText = "Archiveren";
            }
            _boatRepo.UpdateStatus(Boat);
            OnPropertyChanged(nameof(Boat));
            await Shell.Current.GoToAsync($"{nameof(EditBoatView)}?BoatId={boatId}");
        }

        private void LoadBoatDetails(int boatId)
        {
            Boat = _boatRepo.GetById(boatId);

            if (Boat == null)
            {
                return;
            }

            if (Boat.BoatStatus != BoatStatus.Gearchiveerd)
            {
                ChangeBoatStatusButtonText = "Archiveren";
            }
            else
            {
                ChangeBoatStatusButtonText = "Herstellen";
            }
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
