namespace ParkEase.Api.Models;

public class Booking
{
    public int Id { get; set; }
    public int ParkingLotId { get; set; }
    public int VehicleId { get; set; }
    public DateTime BookingDate { get; set; }
    public int DurationHours { get; set; }
    public decimal TotalCost { get; set; }
    public string Status { get; set; } = "Active";
}