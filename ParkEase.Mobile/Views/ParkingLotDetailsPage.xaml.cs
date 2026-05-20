using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class ParkingLotDetailsPage : ContentPage
{
    public ParkingLotDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ParkingLotsViewModel vm)
            await vm.LoadParkingLotsAsync();
    }
}