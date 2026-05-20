namespace ParkEase.Mobile.Models;

public class PaymentCard
{
    public int Id { get; set; }

    public string CardholderName { get; set; } = "";

    public string CardNumber { get; set; } = "";

    public string ExpiryDate { get; set; } = "";

    public string CVV { get; set; } = "";

    public string CardType { get; set; } = "";

    public bool IsDefault { get; set; }
}