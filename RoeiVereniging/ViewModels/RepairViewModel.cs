using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using RoeiVereniging.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RoeiVereniging.ViewModels
{
    public class RepairViewModel : BaseViewModel
    {
        private readonly IBoatRepository _boatRepository;
        private readonly IDamageRepository _damageRepository;
        private readonly GlobalViewModel _global;
        private readonly IAuthService _auth;

        public ObservableCollection<RepairDTO> Repairs { get; } = new();
        public IList<TableColumnDefinition> RepairTableColumns { get; }

        public RepairViewModel(IBoatRepository boatRepository, IDamageRepository damageRepository, GlobalViewModel global, IAuthService auth)
        {
            _boatRepository = boatRepository;
            _damageRepository = damageRepository;
            _global = global;
            _auth = auth;

            LoadRepairs();

            RepairTableColumns = new List<TableColumnDefinition>
            {
                new () { Header = "Schade", BindingPath = "Schade", HeaderType = TableHeaderType.Select },
                new () { Header = "Bootnaam", BindingPath= "BoatName", HeaderType = TableHeaderType.Select},
                new () { Header = "Status", BindingPath = "Status", HeaderType = TableHeaderType.Select },
                new () { Header = "Meldingdatum", BindingPath = "NotificationDate", HeaderType = TableHeaderType.SortDate, StringFormat = "{0:dd-MM-yyyy}"  }
            };
        }

        private RepairDTO MapToDto(Boat boat, Damage? lastDamage)
        {
            RepairDTO dto = new RepairDTO
            {
                BoatId = boat.BoatId,
                BoatName = boat.Name,
                Status = boat.BoatStatus,
                NotificationDate = lastDamage?.ReportedAt
            };

            if (boat.BoatStatus == BoatStatus.Onderhoud || boat.BoatStatus == BoatStatus.Kapot)
            {
                dto.Schade = lastDamage == null ? "Schade" : lastDamage.Description;
            }
            else
            {
                dto.Schade = "Geen schade";
            }

            return dto;
        }

        public void LoadRepairs()
        {
            Repairs.Clear();

            List<Boat> boats = _boatRepository.GetAll();
            List<Damage> damages = _damageRepository.GetAll();

            foreach (var boat in boats)
            {
                var lastDamage = damages
                    .Where(d => d.BoatId == boat.BoatId)
                    .OrderByDescending(d => d.ReportedAt)
                    .FirstOrDefault();

                Repairs.Add(MapToDto(boat, lastDamage));
            }
        }
    }
}
