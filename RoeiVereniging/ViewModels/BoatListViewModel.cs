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

        public IList<TableColumnDefinition> BoatTableColumns { get; }

        public ICommand GoToAddBoatCommand { get; }

        public BoatListViewModel()
        {
            _boatRepository = new BoatRepository();
            LoadBoats();
            GoToAddBoatCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(AddBoatView));
            });

            BoatTableColumns = new List<TableColumnDefinition>
            {
                new () { Header = "Boot toevoegen", BindingPath = "Name", HeaderType = TableHeaderType.Button, Command = GoToAddBoatCommand, Width = "200"},
                new () { Header = "Boot Naam", BindingPath = "Name", HeaderType = TableHeaderType.Select, Width = "*" },
                new () { Header = "Niveau", BindingPath = "Level", HeaderType = TableHeaderType.Select, Width = "*" },
                new () { Header = "Status", BindingPath = "BoatStatus", HeaderType = TableHeaderType.Select, Width = "*" }
            };
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
