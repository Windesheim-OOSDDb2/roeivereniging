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
        private readonly IBoatRepository _boatRepository = new BoatRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();

        public ObservableCollection<RepairDTO> Repairs { get; } = new();
        public IList<TableColumnDefinition> RepairTableColumns { get; }

        public RepairViewModel()
        {
            _damageRepository = new DamageRepository();
            _boatRepository = new BoatRepository();
            LoadRepairs();

            RepairTableColumns = new List<TableColumnDefinition>
            {
                new () { Header = "Schade", BindingPath = "Schade", HeaderType = TableHeaderType.Select },
                new () {Header = "Bootnaam", BindingPath= "BoatName", HeaderType = TableHeaderType.Select},
                new () { Header = "Status", BindingPath = "BoatStatus", HeaderType = TableHeaderType.Select },
                new () { Header = "Meldingdatum", BindingPath = "NotificationDate", HeaderType = TableHeaderType.SortDate, StringFormat = "{0:dd-MM-yyyy}"  }
            };
        }

        public void LoadRepairs()
        {
            Repairs.Clear();

            var boats = _boatRepository.GetAll();
            var damages = _damageRepository.GetAll();

            foreach (var boat in boats)
            {
                var lastDamage = damages
                    .Where(d => d.BoatId == boat.BoatId)
                    .OrderByDescending(d => d.ReportedAt)
                    .FirstOrDefault();

                var dto = new RepairDTO
                {
                    BoatId = boat.BoatId,
                    BoatName = boat.Name,
                    Status = boat.BoatStatus,
                };

                if (boat.BoatStatus == BoatStatus.Fixing || boat.BoatStatus == BoatStatus.Broken)
                {
                    dto.Schade = lastDamage == null ? "Schade" : lastDamage.Description;
                    dto.Notificationdate = lastDamage?.ReportedAt;
                }
                else
                {
                    dto.Schade = "Geen schade";
                    dto.Notificationdate = null;
                }

                Repairs.Add(dto);
            }
        }
    }
}
