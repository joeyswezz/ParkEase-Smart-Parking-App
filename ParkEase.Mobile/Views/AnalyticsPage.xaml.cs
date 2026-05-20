using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class AnalyticsPage : ContentPage
{
    public AnalyticsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AnalyticsViewModel vm)
            await vm.LoadAnalyticsAsync();
    }
}