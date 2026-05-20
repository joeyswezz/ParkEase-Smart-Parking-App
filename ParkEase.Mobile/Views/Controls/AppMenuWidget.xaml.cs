using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.Views.Controls;

public partial class AppMenuWidget : ContentView
{
    public AppMenuWidget()
    {
        InitializeComponent();
        ApplyRoleVisibility();
    }

    private void ApplyRoleVisibility()
    {
        var role = AppState.CurrentUser?.Role ?? "Regular";

        var isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        var isOperator = role.Equals("Operator", StringComparison.OrdinalIgnoreCase);

        AdminMenuItem.IsVisible = isAdmin || isOperator;
        AnalyticsMenuItem.IsVisible = isAdmin || isOperator;
    }

    private async void MenuButton_Tapped(object sender, TappedEventArgs e)
    {
        ApplyRoleVisibility();

        if (MenuPanel.IsVisible)
        {
            await CloseMenuAsync();
            return;
        }

        MenuOverlay.IsVisible = true;
        MenuPanel.IsVisible = true;

        MenuOverlay.Opacity = 0;
        MenuPanel.Opacity = 0;
        MenuPanel.TranslationX = 40;

        await Task.WhenAll(
            MenuOverlay.FadeToAsync(1, 180, Easing.CubicOut),
            MenuPanel.FadeToAsync(1, 220, Easing.CubicOut),
            MenuPanel.TranslateToAsync(0, 0, 220, Easing.CubicOut)
        );
    }

    private async Task CloseMenuAsync()
    {
        await Task.WhenAll(
            MenuOverlay.FadeToAsync(0, 160, Easing.CubicIn),
            MenuPanel.FadeToAsync(0, 160, Easing.CubicIn),
            MenuPanel.TranslateToAsync(40, 0, 160, Easing.CubicIn)
        );

        MenuPanel.IsVisible = false;
        MenuOverlay.IsVisible = false;
    }

    private async Task GoToAsync(string route)
    {
        await CloseMenuAsync();
        await Shell.Current.GoToAsync(route);
    }

    private async void Home_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//Dashboard");
    private async void Map_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//NearbyParking");
    private async void Analytics_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//Analytics");
    private async void Profile_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//Profile");
    private async void Admin_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//Admin");
    private async void Ai_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//AiAssistant");
    private async void Vehicles_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//Vehicles");
    private async void Bookings_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//MyBookingsPage");
    private async void Parking_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//ParkingLots");
    private async void About_Tapped(object sender, TappedEventArgs e) => await GoToAsync("//AboutApp");

    private async void Exit_Tapped(object sender, TappedEventArgs e)
    {
        await CloseMenuAsync();
        AppState.CurrentUser = null;
        await Shell.Current.GoToAsync("//Login");
    }
}