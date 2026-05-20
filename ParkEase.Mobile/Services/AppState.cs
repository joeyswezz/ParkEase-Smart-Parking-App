using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Services;

public static class AppState
{
    public static ParkingLot? SelectedParkingLot { get; set; }

    public static User? CurrentUser { get; set; }

    public static bool IsLoggedIn => CurrentUser is not null;
}