using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RoeiVereniging.ViewModels;
public partial class ReservationDetailViewModel : ObservableObject
{
    private readonly ReservationRepository _reservationRepository = new ReservationRepository();
    private readonly BoatRepository _boatRepo = new BoatRepository();
    private readonly DamageRepository _damageRepository = new DamageRepository();

    // The reservation details will be loaded based on ReservationId
    private ReservationViewDTO _reservation;
    public ReservationViewDTO Reservation
    {
        get => _reservation;
        set => SetProperty(ref _reservation, value);
    }

    [ObservableProperty]
    public string boatDisplayText;

    [ObservableProperty]
    public string steeringModeText;

    [ObservableProperty]
    public string boatLevelText;

    [ObservableProperty]
    public string seatsAmount;

    [ObservableProperty]
    public DateTime startTime;

    [ObservableProperty]
    public DateTime endTime;

    public ObservableCollection<Damage> Damages { get; } = new();

    public ReservationDetailViewModel(int reservationId)
    {
        LoadReservationDetails(reservationId);
        if (Reservation != null)
        {
            LoadDamages(reservationId);
        }
        else
        {
            // Handle the case when Reservation is null
            Debug.WriteLine("Reservation could not be loaded.");
        }
    }

    [RelayCommand]
    public async Task ReportDamage()
    {
        if (Reservation != null)
        {
            int boatId = Reservation.BoatId;
            int reservationId = Reservation.ReservationId;
            await Shell.Current.GoToAsync($"ReportDamageView?BoatId={boatId}&ReservationId={reservationId}");
        }
    }

    [RelayCommand]
    public async Task NavigateToDamageHistory()
    {
        if (Reservation == null)
        {
            Debug.WriteLine("Cannot navigate to damage history: Reservation is null.");
            return;
        }
        await Shell.Current.GoToAsync($"DamageHistoryView?boatId={Reservation.BoatId}");
    }

    private void LoadDamages(int reservationId)
    {
        Damages.Clear();
        var getReservation = _reservationRepository.Get(reservationId);
        if (getReservation == null)
        {
            Debug.WriteLine($"No reservation found for ID: {reservationId}");
            return;
        }

        var boatId = getReservation.BoatId;
        var damages = _damageRepository.GetByBoatId(boatId);

        if (damages != null)
        {
            foreach (var damage in damages)
            {
                Damages.Add(damage);
            }
        }

        OnPropertyChanged(nameof(Damages));
    }

    private void LoadReservationDetails(int reservationId)
    {
        Debug.WriteLine($"Loading reservation details for ID: {reservationId} {reservationId.GetType()}");
        var reservation = _reservationRepository.Get(reservationId);
        if (reservation == null)
        {
            Debug.WriteLine($"No reservation found for ID: {reservationId}");
            return;
        }

        var boat = _boatRepo.Get(reservation.BoatId);
        if (boat == null)
        {
            Debug.WriteLine($"No boat found for ID: {reservation.BoatId}");
            return;
        }

        // Set the Reservation object
        Reservation = new ReservationViewDTO(
            reservation.Id,
            reservation.UserId,
            boat.Level,
            reservation.BoatId,
            boat.Name,
            reservation.StartTime,
            reservation.EndTime,
            boat.SeatsAmount,
            boat.SteeringWheelPosition ? SteeringMode.Required : SteeringMode.Disabled
        );

        boatDisplayText = boat.Name;
        steeringModeText = boat.SteeringWheelPosition.ToString();
        boatLevelText = boat.Level.ToString();
        seatsAmount = boat.SeatsAmount.ToString();
        startTime = Reservation.StartTime;
        endTime = Reservation.EndTime;

        OnPropertyChanged(nameof(boatDisplayText));
        OnPropertyChanged(nameof(steeringModeText));
        OnPropertyChanged(nameof(boatLevelText));
        OnPropertyChanged(nameof(seatsAmount));
        OnPropertyChanged(nameof(Reservation));
        OnPropertyChanged(nameof(startTime));
        OnPropertyChanged(nameof(endTime));
        OnPropertyChanged(nameof(Damages));
    }
}
