using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public static class ParkingLotSeeder
{
    public static async Task SeedAsync()
    {
        var db = new ParkingLotDatabaseService();

        await db.ResetParkingLotsTableAsync();

        var lots = new List<ParkingLot>
        {
            new()
            {
                Name = "New Kingston Secure Parking",
                Location = "New Kingston, Jamaica",
                PricePerHour = 250,
                TotalSpaces = 80,
                AvailableSpaces = 35,
                SecurityLevel = "High",
                OpeningHours = "24 Hours",
                Image = "newkingston_parking.jpg"
            },
            new()
            {
                Name = "Half Way Tree Plaza Parking",
                Location = "Half Way Tree, Jamaica",
                PricePerHour = 180,
                TotalSpaces = 60,
                AvailableSpaces = 22,
                SecurityLevel = "Medium",
                OpeningHours = "7:00 AM - 9:00 PM",
                Image = "halfwaytree_parking.webp"
            },
            new()
            {
                Name = "Waterloo Road Premium Parking",
                Location = "Waterloo Road, Jamaica",
                PricePerHour = 220,
                TotalSpaces = 55,
                AvailableSpaces = 12,
                SecurityLevel = "High",
                OpeningHours = "24 Hours",
                Image = "waterlooroad_parking.jpg"
            },
            new()
            {
                Name = "Downtown Kingston Smart Lot",
                Location = "Downtown Kingston",
                PricePerHour = 150,
                TotalSpaces = 100,
                AvailableSpaces = 48,
                SecurityLevel = "Medium",
                OpeningHours = "24 Hours",
                Image = "downtownkingston_parking.jpg"
            }
        };

        foreach (var lot in lots)
            await db.SaveParkingLotAsync(lot);
    }
}