using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class AdminDashboardViewModel : ObservableObject
{
    private readonly ParkingLotDatabaseService _parkingLotDb = new();

    public ObservableCollection<ParkingLot> ParkingLots { get; } = new();

    [ObservableProperty]
    private int editingLotId = 0;

    [ObservableProperty]
    private string formTitle = "Parking Lot Management";

    [ObservableProperty]
    private string formButtonText = "Add Parking Lot";

    [ObservableProperty]
    private string lotName = "";

    [ObservableProperty]
    private string lotLocation = "";

    [ObservableProperty]
    private string lotTotalSpaces = "";

    [ObservableProperty]
    private string lotAvailableSpaces = "";

    [ObservableProperty]
    private string lotPrice = "";

    [ObservableProperty]
    private string lotSecurity = "";

    [ObservableProperty]
    private string lotHours = "";

    [ObservableProperty]
    private string lotImage = "parking_default.png";

    public AdminDashboardViewModel()
    {
        _ = LoadParkingLotsAsync();
    }

    private async Task LoadParkingLotsAsync()
    {
        ParkingLots.Clear();

        var lots = await _parkingLotDb.GetParkingLotsAsync();

        foreach (var lot in lots)
            ParkingLots.Add(lot);
    }

    [RelayCommand]
    private async Task AddParkingLotAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(LotName) ||
                string.IsNullOrWhiteSpace(LotLocation) ||
                string.IsNullOrWhiteSpace(LotTotalSpaces) ||
                string.IsNullOrWhiteSpace(LotAvailableSpaces) ||
                string.IsNullOrWhiteSpace(LotPrice))
            {
                await Shell.Current.DisplayAlert(
                    "Missing Information",
                    "Please complete the parking lot name, location, total spaces, available spaces, and price.",
                    "OK");

                return;
            }

            var totalSpaces = int.Parse(LotTotalSpaces);
            var availableSpaces = int.Parse(LotAvailableSpaces);

            if (availableSpaces > totalSpaces)
                availableSpaces = totalSpaces;

            var lot = new ParkingLot
            {
                Id = EditingLotId,
                Name = LotName,
                Location = LotLocation,
                TotalSpaces = totalSpaces,
                AvailableSpaces = availableSpaces,
                PricePerHour = decimal.Parse(LotPrice),
                SecurityLevel = string.IsNullOrWhiteSpace(LotSecurity) ? "Medium" : LotSecurity,
                OpeningHours = string.IsNullOrWhiteSpace(LotHours) ? "24 Hours" : LotHours,
                Image = string.IsNullOrWhiteSpace(LotImage) ? "parking_default.png" : LotImage
            };

            await _parkingLotDb.SaveParkingLotAsync(lot);

            await LoadParkingLotsAsync();

            await Shell.Current.DisplayAlert(
                EditingLotId == 0 ? "Success" : "Updated",
                EditingLotId == 0 ? "Parking lot added successfully." : "Parking lot updated successfully.",
                "OK");

            ClearManagementForm();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    [RelayCommand]
    private void EditParkingLot(ParkingLot lot)
    {
        if (lot is null)
            return;

        EditingLotId = lot.Id;

        LotName = lot.Name;
        LotLocation = lot.Location;
        LotTotalSpaces = lot.TotalSpaces.ToString();
        LotAvailableSpaces = lot.AvailableSpaces.ToString();
        LotPrice = lot.PricePerHour.ToString();
        LotSecurity = lot.SecurityLevel;
        LotHours = lot.OpeningHours;
        LotImage = lot.Image;

        FormTitle = "Edit Parking Lot";
        FormButtonText = "Update Parking Lot";
    }

    [RelayCommand]
    private async Task DeleteParkingLotAsync(ParkingLot lot)
    {
        if (lot is null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Delete Parking Lot",
            $"Are you sure you want to delete {lot.Name}?",
            "Yes",
            "No");

        if (!confirm)
            return;

        await _parkingLotDb.DeleteParkingLotAsync(lot);

        await LoadParkingLotsAsync();

        await Shell.Current.DisplayAlert(
            "Deleted",
            "Parking lot deleted successfully.",
            "OK");
    }

    [RelayCommand]
    private void ClearManagementForm()
    {
        EditingLotId = 0;
        FormTitle = "Parking Lot Management";
        FormButtonText = "Add Parking Lot";

        LotName = "";
        LotLocation = "";
        LotTotalSpaces = "";
        LotAvailableSpaces = "";
        LotPrice = "";
        LotSecurity = "";
        LotHours = "";
        LotImage = "parking_default.png";
    }

    public void UpdateLotImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return;

        LotImage = imagePath;
    }
}