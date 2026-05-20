namespace ParkEase.Api.Models;

public class ParkingAssistantRequest
{
    public string UserQuestion { get; set; } = "";
    public string ParkingLotsJson { get; set; } = "";
    public string VehiclesJson { get; set; } = "";
    public string BookingsJson { get; set; } = "";
}