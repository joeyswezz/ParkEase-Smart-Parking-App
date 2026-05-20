using ParkEase.Mobile.ViewModels;
using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();

        Opacity = 0;
        TranslationY = 30;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DashboardViewModel vm)
        {
            await vm.LoadDashboardDataAsync();
        }

        AppNotificationService.Show(
            "Welcome to ParkEase",
            "Your dashboard is ready.");

        await Task.WhenAll(
            this.FadeToAsync(1, 800, Easing.CubicOut),
            this.TranslateToAsync(0, 0, 800, Easing.CubicOut)
        );
    }

    private async void VehiclesCard_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Vehicles");
    }

    private async void NearbyCard_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//NearbyParking");
    }

    private async void ReserveCard_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//MyBookingsPage");
    }

    private async void HistoryCard_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//MyBookingsPage");
    }

    private async void FeaturedReserve_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MyBookingsPage");
    }

    private async void ReserveButton_Pressed(object sender, EventArgs e)
    {
        if (sender is Button btn)
            await btn.ScaleToAsync(0.95, 80);
    }

    private async void ReserveButton_Released(object sender, EventArgs e)
    {
        if (sender is Button btn)
            await btn.ScaleToAsync(1, 80);
    }
}