using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Data.Repositories;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using RoeiVereniging.Views;


namespace RoeiVereniging.ViewModels
{
    public class BoatListViewModel : INotifyPropertyChanged
    {
        private readonly BoatRepository _boatRepository;
        public ObservableCollection<Boat> Boats { get; } = new();

        public IList<TableColumnDefinition> BoatTableColumns { get; } = new List<TableColumnDefinition>
        {
            new TableColumnDefinition { Header = "Id", BindingPath = "BoatId", Width = "Auto", HeaderType = TableHeaderType.Text },
            new TableColumnDefinition { Header = "Naam", BindingPath = "Name", Width = "*", HeaderType = TableHeaderType.Text },
            new TableColumnDefinition { Header = "Level", BindingPath = "Level", Width = "*", HeaderType = TableHeaderType.Text },
            new TableColumnDefinition { Header = "Status", BindingPath = "BoatStatus", Width = "*", HeaderType = TableHeaderType.Text }
        };

        public ICommand GoToAddBoatCommand { get; }

        public BoatListViewModel()
        {
            _boatRepository = new BoatRepository();
            LoadBoats();
            GoToAddBoatCommand = new RelayCommand(OnGoToAddBoat);
        }

        public void LoadBoats()
        {
            Boats.Clear();
            foreach (var boat in _boatRepository.GetAll())
            {
                Boats.Add(boat);
            }
        }

        public void Refresh()
        {
            LoadBoats();
        }

        private async void OnGoToAddBoat()
        {
            await Shell.Current.GoToAsync(nameof(AddBoatView));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
