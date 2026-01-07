using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserDetailView : ContentPage
{
    private int _userId;
    private readonly IUserRepository _userRepository;
    private readonly IDamageRepository _damageRepository;
    private readonly IReservationRepository _reservationRepository;

    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            // works I guess? doesn't look very nice tho
            BindingContext = new UserDetailViewModel(
                _userId,
                _userRepository,
                _damageRepository,
                _reservationRepository
            );
        }
    }

    public UserDetailView(
        IUserRepository userRepository,
        IDamageRepository damageRepository,
        IReservationRepository reservationRepository
    )
    {
        _userRepository = userRepository;
        _damageRepository = damageRepository;
        _reservationRepository = reservationRepository;
        InitializeComponent();
    }
}