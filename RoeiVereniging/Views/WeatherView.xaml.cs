using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class WeatherView : ContentPage
{
    public WeatherView(WeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}