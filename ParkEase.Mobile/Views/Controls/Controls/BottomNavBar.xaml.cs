namespace ParkEase.Mobile.Views.Controls;

public partial class BottomNavBar : ContentView
{
    public BottomNavBar()
    {
        InitializeComponent();
    }

    private async Task AnimateItemAsync(View item)
    {
        await item.TranslateTo(0, -10, 120, Easing.CubicOut);
        await item.ScaleTo(1.12, 120, Easing.CubicOut);

        await Task.Delay(80);

        await item.ScaleTo(1, 120, Easing.CubicIn);
        await item.TranslateTo(0, 0, 120, Easing.CubicIn);
    }

    private async void Home_Tapped(object sender, TappedEventArgs e)
    {
        await AnimateItemAsync(HomeItem);
        await Shell.Current.GoToAsync("//Dashboard");
    }

    private async void Map_Tapped(object sender, TappedEventArgs e)
    {
        await AnimateItemAsync(MapItem);
        await Shell.Current.GoToAsync("//NearbyParking");
    }

    private async void Parking_Tapped(object sender, TappedEventArgs e)
    {
        await AnimateItemAsync(ParkingItem);
        await Shell.Current.GoToAsync("//ParkingLots");
    }

    private async void Bookings_Tapped(object sender, TappedEventArgs e)
    {
        await AnimateItemAsync(BookingsItem);
        await Shell.Current.GoToAsync("//MyBookingsPage");
    }
}