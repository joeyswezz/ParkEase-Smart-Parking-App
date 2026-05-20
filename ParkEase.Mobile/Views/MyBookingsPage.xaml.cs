using ParkEase.Mobile.Services;
using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class MyBookingsPage : ContentPage
{
    public MyBookingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppState.SelectedParkingLot is not null &&
            BindingContext is BookingsViewModel vm)
        {
            vm.ApplySelectedParkingLot(AppState.SelectedParkingLot);
            AppState.SelectedParkingLot = null;
        }
    }
}