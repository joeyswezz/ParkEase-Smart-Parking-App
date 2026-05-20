namespace ParkEase.Mobile.Views;

public partial class NearbyParkingPage : ContentPage
{
    private Location? _currentLocation;

    public NearbyParkingPage()
    {
        InitializeComponent();
    }

    private async void UseMyLocation_Clicked(object sender, EventArgs e)
    {
        try
        {
            LocationStatusLabel.Text = "Getting your current location...";

            var permission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (permission != PermissionStatus.Granted)
                permission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (permission != PermissionStatus.Granted)
            {
                LocationStatusLabel.Text = "Location permission was not granted.";
                return;
            }

            var request = new GeolocationRequest(
                GeolocationAccuracy.Medium,
                TimeSpan.FromSeconds(10));

            _currentLocation = await Geolocation.Default.GetLocationAsync(request);

            if (_currentLocation is null)
            {
                LocationStatusLabel.Text = "Could not detect your location.";
                return;
            }

            LocationStatusLabel.Text =
                $"Location found: {_currentLocation.Latitude:F5}, {_currentLocation.Longitude:F5}";

            var url =
                $"https://www.google.com/maps/search/parking/@{_currentLocation.Latitude},{_currentLocation.Longitude},15z";

            MapWebView.Source = url;
        }
        catch (Exception ex)
        {
            LocationStatusLabel.Text = $"Location error: {ex.Message}";
        }
    }

    private async void OpenGoogleMaps_Clicked(object sender, EventArgs e)
    {
        var url = "https://www.google.com/maps/search/parking+near+Kingston+Jamaica";
        await Launcher.OpenAsync(url);
    }

    private async void DirectionsToNewKingston_Clicked(object sender, EventArgs e)
    {
        var destination = Uri.EscapeDataString("New Kingston Secure Parking, Kingston Jamaica");

        string url;

        if (_currentLocation is not null)
        {
            url =
                $"https://www.google.com/maps/dir/?api=1&origin={_currentLocation.Latitude},{_currentLocation.Longitude}&destination={destination}&travelmode=driving";
        }
        else
        {
            url =
                $"https://www.google.com/maps/dir/?api=1&destination={destination}&travelmode=driving";
        }

        await Launcher.OpenAsync(url);
    }
}