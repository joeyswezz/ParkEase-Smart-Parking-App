using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;
using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ParkingLotDatabaseService _parkingLotDb = new();
    private readonly BookingDatabaseService _bookingDb = new();

    [ObservableProperty]
    private string welcomeText = "Welcome Back";

    [ObservableProperty]
    private string displayName = "";

    [ObservableProperty]
    private string profileImage = "profile_placeholder.png";

    [ObservableProperty]
    private int availableLots;

    [ObservableProperty]
    private int myBookings;

    public DashboardViewModel()
    {
        _ = LoadDashboardDataAsync();
    }

    public async Task LoadDashboardDataAsync()
    {
        LoadUser();

        await LoadParkingStatsAsync();
        await LoadBookingStatsAsync();
    }

    public void LoadUser()
    {
        var user = AppState.CurrentUser;

        if (user is null)
        {
            WelcomeText = "Welcome";
            DisplayName = "Guest";
            ProfileImage = "profile_placeholder.png";
            return;
        }

        WelcomeText = $"Welcome Back, {user.FirstName} 👋";
        DisplayName = $"@{user.DisplayName}";
        ProfileImage = user.ProfileImage;
    }

    private async Task LoadParkingStatsAsync()
    {
        var lots = await _parkingLotDb.GetParkingLotsAsync();

        AvailableLots = lots.Count(l => l.AvailableSpaces > 0);
    }

    private async Task LoadBookingStatsAsync()
    {
        var user = AppState.CurrentUser;

        if (user is null)
        {
            MyBookings = 0;
            return;
        }

        var bookings = await _bookingDb.GetBookingsAsync();

        MyBookings = bookings.Count(b =>
            b.Status != "Cancelled" &&
            b.Status != "Completed");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        AppState.CurrentUser = null;

        await Shell.Current.GoToAsync("//Login");
    }
}