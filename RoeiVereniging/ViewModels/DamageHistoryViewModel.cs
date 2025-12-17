using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using RoeiVereniging.Core.Data.Repositories;

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(BoatId), "boatId")]
    public partial class DamageHistoryViewModel : ObservableObject
    {
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
                    OnPropertyChanged(nameof(BoatDisplayText));
                }
            }
        }

        private string boatName;
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
    }
}
