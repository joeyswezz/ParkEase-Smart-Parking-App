using QRCoder;

namespace ParkEase.Mobile.Views;

public partial class BookingConfirmationPage : ContentPage
{
    public BookingConfirmationPage(
        string parkingLot,
        string plateNumber,
        string duration)
    {
        InitializeComponent();

        ParkingLotLabel.Text = parkingLot;
        PlateNumberLabel.Text = plateNumber;
        DurationLabel.Text = duration;

        var reservationId = $"PE-{Random.Shared.Next(100000, 999999)}";
        ReservationIdLabel.Text = reservationId;

        var transactionId = $"TXN-{Random.Shared.Next(1000000, 9999999)}";
        TransactionIdLabel.Text = transactionId;

        PaymentTimeLabel.Text = DateTime.Now.ToString("MMM dd, yyyy • hh:mm tt");

        var randomAmount = Random.Shared.Next(250, 15000);
        AmountPaidLabel.Text = $"${randomAmount:N0}";

        GenerateQrCode(
            reservationId,
            parkingLot,
            plateNumber,
            duration,
            transactionId);
    }

    private void GenerateQrCode(
        string reservationId,
        string parkingLot,
        string plateNumber,
        string duration,
        string transactionId)
    {
        var qrText =
$"""
PARK EASE RESERVATION
Reservation: {reservationId}
Transaction: {transactionId}
Parking Lot: {parkingLot}
Vehicle: {plateNumber}
Duration: {duration}
Generated: {DateTime.Now}
""";

        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);

        var qrBytes = qrCode.GetGraphic(20);

        QrImage.Source = ImageSource.FromStream(() => new MemoryStream(qrBytes));
    }

    private async void DoneButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Dashboard");
    }
}