using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Views;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Services;
using ParkEase.Mobile.Models;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class BookingsViewModel : ObservableObject
{
    private readonly BookingDatabaseService _bookingDb = new();
    private readonly VehicleDatabaseService _vehicleDb = new();
    private readonly ParkingLotDatabaseService _parkingLotDb = new();

    public ObservableCollection<Booking> Bookings { get; } = new();
    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    public ObservableCollection<string> ParkingLots { get; } = new()
    {
        "New Kingston Secure Parking",
        "Half Way Tree Plaza Parking",
        "Waterloo Road Premium Parking",
        "Downtown Kingston Smart Lot"
    };

    public void ApplySelectedParkingLot(ParkingLot lot)
    {
        SelectedParkingLot = lot.Name;
        PricePerHour = lot.PricePerHour;
        UpdateTotal();
    }

    [ObservableProperty]
    private string selectedParkingLot = "";

    [ObservableProperty]
    private Vehicle? selectedVehicle;

    [ObservableProperty]
    private DateTime bookingDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan bookingTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private int durationHours = 1;

    [ObservableProperty]
    private decimal pricePerHour = 250;

    [ObservableProperty]
    private string notes = "";

    [ObservableProperty]
    private string calculatedTotal = "$250";

    public BookingsViewModel()
    {
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        Bookings.Clear();
        Vehicles.Clear();

        var savedBookings = await _bookingDb.GetBookingsAsync();

        foreach (var booking in savedBookings.OrderByDescending(b => b.BookingDate))
            Bookings.Add(booking);

        var savedVehicles = await _vehicleDb.GetVehiclesAsync();

        foreach (var vehicle in savedVehicles)
            Vehicles.Add(vehicle);

        if (string.IsNullOrWhiteSpace(SelectedParkingLot))
            SelectedParkingLot = ParkingLots.FirstOrDefault() ?? "";

        UpdateTotal();
    }

    [RelayCommand]
    private async Task CreateBookingAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedParkingLot) || SelectedVehicle is null)
            return;

        if (DurationHours <= 0)
            DurationHours = 1;

        var parkingLotName = SelectedParkingLot;
        var plateNumber = SelectedVehicle.PlateNumber;
        var savedDuration = DurationHours;
        var savedRate = PricePerHour;

        var finalDateTime = BookingDate.Date.Add(BookingTime);

        var booking = new Booking
        {
            ParkingLotName = parkingLotName,
            VehiclePlateNumber = plateNumber,
            VehicleImage = SelectedVehicle.VehicleImage,
            BookingDate = finalDateTime,
            DurationHours = savedDuration,
            PricePerHour = savedRate,
            TotalCost = savedDuration * savedRate,
            Status = "Pending Payment",
            Notes = Notes
        };

        await _bookingDb.SaveBookingAsync(booking);
        AppNotificationService.Show(
    "Booking Created",
    $"{parkingLotName} was added to checkout.");

        await LoadDataAsync();

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PushAsync(
                new CheckoutPage(
                    parkingLotName,
                    plateNumber,
                    savedDuration,
                    savedRate)
            );
        });

        Notes = "";
        DurationHours = 1;
        UpdateTotal();
    }

    [RelayCommand]
    private async Task MarkReservedAsync(Booking booking)
    {
        if (booking is null)
            return;

        booking.Status = "Reserved";
        await _bookingDb.SaveBookingAsync(booking);
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task CheckInAsync(Booking booking)
    {
        if (booking is null)
            return;

        booking.Status = "Checked In";
        await _bookingDb.SaveBookingAsync(booking);
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task CompleteBookingAsync(Booking booking)
    {
        if (booking is null)
            return;

        booking.Status = "Completed";

        await RestoreParkingSpaceAsync(booking.ParkingLotName);

       await _bookingDb.SaveBookingAsync(booking);

AppNotificationService.Show(
    "Booking Completed",
    "Parking session completed successfully.");

await LoadDataAsync();
    }

    [RelayCommand]
    private async Task CancelBookingAsync(Booking booking)
    {
        if (booking is null)
            return;

        if (booking.Status != "Cancelled" && booking.Status != "Completed")
        {
            await RestoreParkingSpaceAsync(booking.ParkingLotName);
        }

        booking.Status = "Cancelled";

        await _bookingDb.SaveBookingAsync(booking);

AppNotificationService.Show(
    "Booking Cancelled",
    "Your parking reservation was cancelled.");

await LoadDataAsync();
    }

    [RelayCommand]
    private async Task DeleteBookingAsync(Booking booking)
    {
        if (booking is null)
            return;

        await _bookingDb.DeleteBookingAsync(booking);
        await LoadDataAsync();
    }

    private async Task RestoreParkingSpaceAsync(string parkingLotName)
    {
        var parkingLots = await _parkingLotDb.GetParkingLotsAsync();
        var matchingLot = parkingLots.FirstOrDefault(x => x.Name == parkingLotName);

        if (matchingLot is not null && matchingLot.AvailableSpaces < matchingLot.TotalSpaces)
        {
            matchingLot.AvailableSpaces++;
            await _parkingLotDb.SaveParkingLotAsync(matchingLot);
        }
    }

    partial void OnDurationHoursChanged(int value)
    {
        UpdateTotal();
    }

    partial void OnPricePerHourChanged(decimal value)
    {
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        if (DurationHours <= 0)
            DurationHours = 1;

        var total = DurationHours * PricePerHour;
        CalculatedTotal = $"${total:N0}";
    }
}