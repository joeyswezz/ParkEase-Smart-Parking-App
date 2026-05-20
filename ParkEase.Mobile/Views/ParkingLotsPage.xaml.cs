using ParkEase.Mobile.Models;
using ParkEase.Mobile.Services;
using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class ParkingLotsPage : ContentPage
{
    public ParkingLotsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ParkingLotsViewModel vm)
            await vm.LoadParkingLotsAsync();
    }

    private async void ViewMapButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string address)
        {
            var encodedAddress = Uri.EscapeDataString(address);
            var url = $"https://www.google.com/maps/search/?api=1&query={encodedAddress}";

            await Launcher.OpenAsync(url);
        }
    }

    private async void BookNowButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ParkingLot lot)
        {
            AppState.SelectedParkingLot = lot;
           await Shell.Current.GoToAsync("//MyBookingsPage");
        }
    }
}