using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class AnalyticsViewModel : ObservableObject
{
    private readonly BookingDatabaseService _bookingDb = new();
    private readonly ParkingLotDatabaseService _parkingLotDb = new();

    public ObservableCollection<LotOccupancyItem> LotOccupancyItems { get; } = new();

    [ObservableProperty]
    private string totalRevenue = "$0";

    [ObservableProperty]
    private int totalBookings;

    [ObservableProperty]
    private int activeBookings;

    [ObservableProperty]
    private int cancelledBookings;

    [ObservableProperty]
    private double overallOccupancyRate;

    [ObservableProperty]
    private string overallOccupancyText = "0% used";

    [ObservableProperty]
    private string occupancySummary = "No occupancy data available yet.";

    [ObservableProperty]
    private string insightOne = "• No insights available yet.";

    [ObservableProperty]
    private string insightTwo = "";

    [ObservableProperty]
    private string insightThree = "";

    public AnalyticsViewModel()
    {
        _ = LoadAnalyticsAsync();
    }

    [RelayCommand]
    public async Task LoadAnalyticsAsync()
    {
        var bookings = await _bookingDb.GetBookingsAsync();
        var lots = await _parkingLotDb.GetParkingLotsAsync();

        LotOccupancyItems.Clear();

        foreach (var lot in lots.OrderByDescending(l => l.OccupancyRate))
        {
            LotOccupancyItems.Add(new LotOccupancyItem
            {
                Name = lot.Name,
                AvailableText = $"{lot.AvailableSpaces} of {lot.TotalSpaces} available",
                OccupancyText = $"{lot.OccupancyPercentage:F0}% used",
                OccupancyRate = lot.OccupancyRate,
                StatusLabel = lot.StatusLabel,
                StatusColor = lot.StatusColor
            });
        }

        TotalBookings = bookings.Count;

        ActiveBookings = bookings.Count(b =>
            b.Status == "Reserved" ||
            b.Status == "Checked In" ||
            b.Status == "Pending Payment");

        CancelledBookings = bookings.Count(b => b.Status == "Cancelled");

        var revenue = bookings
            .Where(b => b.Status != "Cancelled")
            .Sum(b => b.TotalCost);

        TotalRevenue = $"${revenue:N0}";

        var totalSpaces = lots.Sum(l => l.TotalSpaces);
        var availableSpaces = lots.Sum(l => l.AvailableSpaces);
        var usedSpaces = totalSpaces - availableSpaces;

        OverallOccupancyRate = totalSpaces == 0 ? 0 : (double)usedSpaces / totalSpaces;
        OverallOccupancyText = $"{OverallOccupancyRate * 100:F0}% used";

        OccupancySummary = $"{usedSpaces} of {totalSpaces} spaces are currently occupied or reserved.";

        var busiestLot = lots
            .OrderByDescending(l => l.OccupancyRate)
            .FirstOrDefault();

        var cheapestLot = lots
            .OrderBy(l => l.PricePerHour)
            .FirstOrDefault();

        InsightOne = busiestLot is null
            ? "• No parking lot data available."
            : $"• {busiestLot.Name} currently has the highest occupancy at {busiestLot.OccupancyPercentage:F0}%.";

        InsightTwo = cheapestLot is null
            ? ""
            : $"• {cheapestLot.Name} is currently the lowest-cost option at ${cheapestLot.PricePerHour:N0}/hr.";

        InsightThree = CancelledBookings > 0
            ? $"• {CancelledBookings} booking(s) have been cancelled and should be reviewed for service trends."
            : "• No cancellations recorded yet. Booking performance looks healthy.";
    }
}