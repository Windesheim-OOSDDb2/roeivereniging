using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using RoeiVereniging.Core.Models;
using RoeiVereniging.ViewModels;

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
            if (query.TryGetValue("Reservation", out var reservationObj) && reservationObj is ReservationViewDTO reservation)
            {
                BindingContext = new ReservationDetailViewModel(reservation);
            }
        }
    }
}
