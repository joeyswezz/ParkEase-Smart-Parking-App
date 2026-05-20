using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class VehiclesPage : ContentPage
{
    public VehiclesPage()
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