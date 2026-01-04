using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserDetailView : ContentPage
{
    private int _userId;
    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            BindingContext = new UserDetailViewModel(_userId);
        }
    }
    public UserDetailView()
	{
		InitializeComponent();
    }
}