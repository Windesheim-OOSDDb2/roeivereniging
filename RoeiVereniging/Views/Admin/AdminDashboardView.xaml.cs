using RoeiVereniging.ViewModels.admin;

namespace RoeiVereniging.Views.Admin;

public partial class AdminDashboardView : ContentPage
{
    private readonly AdminDashboardViewModel _viewModel;
    public AdminDashboardView(AdminDashboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}