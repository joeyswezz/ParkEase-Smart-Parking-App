namespace ParkEase.Api.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = "";
    public string PlateNumber { get; set; } = "";
    public string VehicleType { get; set; } = "";
    public string Color { get; set; } = "";
}