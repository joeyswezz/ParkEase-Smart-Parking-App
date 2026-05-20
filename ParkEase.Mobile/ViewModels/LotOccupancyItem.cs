namespace ParkEase.Mobile.ViewModels;

public class LotOccupancyItem
{
    public string Name { get; set; } = "";
    public string AvailableText { get; set; } = "";
    public string OccupancyText { get; set; } = "";
    public double OccupancyRate { get; set; }
    public string StatusLabel { get; set; } = "";
    public string StatusColor { get; set; } = "";
}