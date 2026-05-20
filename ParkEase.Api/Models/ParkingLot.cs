namespace ParkEase.Api.Models;

public class ParkingLot
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public decimal PricePerHour { get; set; }
    public int TotalSpaces { get; set; }
    public int AvailableSpaces { get; set; }
    public string SecurityLevel { get; set; } = "Medium";
    public string OpeningHours { get; set; } = "8:00 AM - 10:00 PM";
    public string ImageUrl { get; set; } = "";
}