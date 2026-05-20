using SQLite;

namespace ParkEase.Mobile.Models;

public class Vehicle
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public string PhoneNumber { get; set; } = "";
    public string SmsNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";

    public string PlateNumber { get; set; } = "";
    public string VehicleType { get; set; } = "";
    public string Color { get; set; } = "";

    public string VehicleDetails { get; set; } = "";

    public string ProfileImage { get; set; } = "profile_placeholder.png";

    public string VehicleImage { get; set; } = "vehicle_banner.png";

    public DateTime DateAdded { get; set; } = DateTime.Now;
}