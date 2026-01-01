
using RoeiVereniging.ViewModels;
using System.Diagnostics;
namespace RoeiVereniging.Views
{
    public partial class ReservationDetailView : ContentPage, IQueryAttributable
    {
        public ReservationDetailView()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine("Query parameters: " + string.Join(", ", query.Select(kvp => $"{kvp.Key}: {kvp.Value}")));

            if (query.TryGetValue("id", out var stringId) && int.TryParse(stringId.ToString(), out int reservationId))
            {
                Debug.WriteLine($"Successfully received ReservationId (converted): {reservationId}");
                BindingContext = new ReservationDetailViewModel(reservationId);
            }
            else
            {
                Debug.WriteLine("invalid param");
            }
        }



        private async void OnReportDamageClicked(object sender, EventArgs e)
        {
            if (BindingContext is ReservationDetailViewModel vm && vm.Reservation != null)
            {
                int boatId = vm.Reservation.BoatId;
                int reservationId = vm.Reservation.ReservationId;
                await Shell.Current.GoToAsync($"ReportDamageView?BoatId={boatId}&ReservationId={reservationId}");
            }
        }

        private async void OnNavigateToDamageHistoryClicked(object sender, EventArgs e)
        {
            if (BindingContext is ReservationDetailViewModel vm)
            {
                int reservationId = vm.Reservation.ReservationId;
                await Shell.Current.GoToAsync($"DamageHistoryView?ReservationId={reservationId}");
            }
        }
    }
}
