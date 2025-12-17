using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views
{
    public partial class ReportDamageView : ContentPage
    {
        public ReportDamageView()
        {
            InitializeComponent();
            BindingContext = new ReportDamageViewModel();
        }

        private async void OnNavigateToDamageHistoryClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("DamageHistoryView");
        }
    }
}
