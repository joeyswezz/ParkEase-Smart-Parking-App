using SQLite;

namespace ParkEase.Mobile.Models;

public class Booking
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ParkingLotName { get; set; } = "";

    public string VehiclePlateNumber { get; set; } = "";

    public string VehicleImage { get; set; } = "vehicle_banner.png";

    public DateTime BookingDate { get; set; } = DateTime.Now;

    public int DurationHours { get; set; } = 1;

    public decimal PricePerHour { get; set; }

    public decimal TotalCost { get; set; }

    public string Status { get; set; } = "Pending Payment";

    public string Notes { get; set; } = "";

    [Ignore]
    public DateTime EndTime => BookingDate.AddHours(DurationHours);

    [Ignore]
    public string TimerStatus
    {
        get
        {
            var now = DateTime.Now;

            if (Status == "Cancelled")
                return "Cancelled";

            if (Status == "Completed")
                return "Completed";

            if (now < BookingDate)
            {
                var startsIn = BookingDate - now;

                if (startsIn.TotalHours >= 1)
                    return $"Starts in {(int)startsIn.TotalHours}h {startsIn.Minutes}m";

                return $"Starts in {startsIn.Minutes}m";
            }

            if (now >= BookingDate && now <= EndTime)
            {
                var remaining = EndTime - now;

                if (remaining.TotalHours >= 1)
                    return $"Active: {(int)remaining.TotalHours}h {remaining.Minutes}m left";

                return $"Active: {remaining.Minutes}m left";
            }

            return "Expired";
        }
    }
}