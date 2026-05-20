using ParkEase.Mobile.Data;

namespace ParkEase.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        _ = InitializeDataAsync();
    }

    private async Task InitializeDataAsync()
    {
        await ParkingLotSeeder.SeedAsync();
    }
}