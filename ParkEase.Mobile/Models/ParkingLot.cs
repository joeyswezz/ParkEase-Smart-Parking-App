using SQLite;

namespace ParkEase.Mobile.Models;

public class ParkingLot
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public string Location { get; set; } = "";

    public decimal PricePerHour { get; set; }

    public int TotalSpaces { get; set; }
    public int AvailableSpaces { get; set; }

    public string SecurityLevel { get; set; } = "";
    public string OpeningHours { get; set; } = "";
    public string Image { get; set; } = "";

    public bool IsFull => AvailableSpaces <= 0;

    public string StatusLabel
{
    get
    {
        if (AvailableSpaces <= 0)
            return "FULL";

        if (OccupancyPercentage >= 85)
            return "LIMITED";

        if (OccupancyPercentage >= 65)
            return "HIGH DEMAND";

        return "AVAILABLE";
    }
}

public string StatusColor
{
    get
    {
        if (AvailableSpaces <= 0)
            return "#FF3B5C";

        if (OccupancyPercentage >= 85)
            return "#FFB84D";

        if (OccupancyPercentage >= 65)
            return "#A855F7";

        return "#2BAE66";
    }
}

    public double OccupancyRate =>
        TotalSpaces == 0 ? 0 : (double)(TotalSpaces - AvailableSpaces) / TotalSpaces;

    public double OccupancyPercentage => OccupancyRate * 100;
}