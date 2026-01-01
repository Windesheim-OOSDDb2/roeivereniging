using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(BoatId), "boatId")]
    public partial class DamageHistoryViewModel : ObservableObject
    {
        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly BoatRepository _boatRepository = new BoatRepository();

        private int boatId;
        public int BoatId
        {
            get => boatId;
            set
            {
                if (SetProperty(ref boatId, value))
                {
                    var boat = _boatRepository.Get(boatId);
                    BoatName = boat?.ToString() ?? boatId.ToString();
                    LoadDamages();
                    OnPropertyChanged(nameof(BoatDisplayText));
                }
            }
        }

        private string boatName = string.Empty;
        public string BoatName
        {
            get => boatName;
            set
            {
                if (SetProperty(ref boatName, value))
                    OnPropertyChanged(nameof(BoatDisplayText));
            }
        }

        public string BoatDisplayText => $"Historie van schade meldingen van boot: {BoatName} (ID: {BoatId})";

        public ObservableCollection<Damage> Damages { get; } = new();

        private void LoadDamages()
        {
            Damages.Clear();
            var damages = _damageRepository.GetByBoatId(BoatId)
                .OrderByDescending(d => d.ReportedAt);
            foreach (var damage in damages)
            {
                Damages.Add(damage);
            }
        }
    }
}
