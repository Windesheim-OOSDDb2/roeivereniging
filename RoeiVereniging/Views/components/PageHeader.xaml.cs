using RoeiVereniging.Views;

namespace RoeiVereniging.Views.Components;

public partial class PageHeader : ContentView
{
    public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(
        nameof(PageTitle), typeof(string), typeof(PageHeader), ""
    );

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

    public static readonly BindableProperty BackTargetProperty = BindableProperty.Create(
        nameof(BackTarget), typeof(string), typeof(PageHeader), ""
    );

    public string BackTarget
    {
        get => (string)GetValue(BackTargetProperty);
        set => SetValue(BackTargetProperty, value);
    }

    public PageHeader()
    {
        InitializeComponent();
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReserveBoatView));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(BackTarget))
        {
            await Shell.Current.GoToAsync(BackTarget);
            return;
        }

        if (Shell.Current.Navigation.NavigationStack.Count > 1)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}