namespace ParkEase.Mobile.Views;
using ParkEase.Mobile.Services;

public partial class CheckoutPage : ContentPage
{
    private readonly string _parkingLot;
    private readonly string _plateNumber;
    private readonly int _durationHours;
    private readonly decimal _pricePerHour;

    public CheckoutPage(string parkingLot, string plateNumber, int durationHours, decimal pricePerHour)
    {
        InitializeComponent();

        _parkingLot = parkingLot;
        _plateNumber = plateNumber;
        _durationHours = durationHours;
        _pricePerHour = pricePerHour;

        LoadCheckoutDetails();
    }

    private void LoadCheckoutDetails()
    {
        var subtotal = _durationHours * _pricePerHour;
        var fee = 50m;
        var total = subtotal + fee;

        ParkingLotLabel.Text = _parkingLot;
        PlateLabel.Text = _plateNumber;
        DurationLabel.Text = $"{_durationHours} hour(s)";
        RateLabel.Text = $"${_pricePerHour:N0}/hr";

        SubtotalLabel.Text = $"${subtotal:N0}";
        FeeLabel.Text = $"${fee:N0}";
        TotalLabel.Text = $"${total:N0}";
    }

    private async void PayButton_Clicked(object sender, EventArgs e)
    {
        ProcessingIndicator.IsVisible = true;
        ProcessingIndicator.IsRunning = true;
        StatusLabel.Text = "Processing payment...";

        await Task.Delay(1800);

        ProcessingIndicator.IsRunning = false;
        ProcessingIndicator.IsVisible = false;
        StatusLabel.Text = "Payment approved.";
        AppNotificationService.Show(
    "Payment Approved",
    "Your reservation payment was successful.");

        await Task.Delay(700);

        await Shell.Current.Navigation.PushAsync(
            new BookingConfirmationPage(
                _parkingLot,
                _plateNumber,
                $"{_durationHours} hour(s)")
        );
    }
}