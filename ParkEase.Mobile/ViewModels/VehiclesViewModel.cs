using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class VehiclesViewModel : ObservableObject
{
    private readonly VehicleDatabaseService _databaseService = new();

    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    [ObservableProperty]
    private int editingVehicleId;

    [ObservableProperty]
    private string firstName = "";

    [ObservableProperty]
    private string lastName = "";

    [ObservableProperty]
    private string phoneNumber = "";

    [ObservableProperty]
    private string smsNumber = "";

    [ObservableProperty]
    private string emailAddress = "";

    [ObservableProperty]
    private string plateNumber = "";

    [ObservableProperty]
    private string vehicleType = "";

    [ObservableProperty]
    private string color = "";

    [ObservableProperty]
    private string vehicleDetails = "";

    [ObservableProperty]
    private string formTitle = "Add Vehicle";

    [ObservableProperty]
    private string saveButtonText = "Add Vehicle";

    public VehiclesViewModel()
    {
        _ = LoadVehiclesAsync();
    }

    [RelayCommand]
    private async Task LoadVehiclesAsync()
    {
        Vehicles.Clear();

        var savedVehicles = await _databaseService.GetVehiclesAsync();

        foreach (var vehicle in savedVehicles)
            Vehicles.Add(vehicle);
    }

    [RelayCommand]
    private async Task SaveVehicleAsync()
    {
        if (string.IsNullOrWhiteSpace(PlateNumber))
            return;

        string selectedBanner = "vehicle_banner.png";

        var random = new Random();
        int bannerChoice = random.Next(1, 4);

        switch (bannerChoice)
        {
            case 1:
                selectedBanner = "vehicle_banner.png";
                break;

            case 2:
                selectedBanner = "vehicle_banner2.png";
                break;

            case 3:
                selectedBanner = "vehicle_banner3.png";
                break;
        }

        var vehicle = new Vehicle
        {
            Id = EditingVehicleId,

            FirstName = FirstName,
            LastName = LastName,

            PhoneNumber = PhoneNumber,
            SmsNumber = SmsNumber,
            EmailAddress = EmailAddress,

            PlateNumber = PlateNumber,
            VehicleType = VehicleType,
            Color = Color,

            VehicleDetails = VehicleDetails,

            ProfileImage = "profile_placeholder.png",

            VehicleImage = selectedBanner,

            DateAdded = DateTime.Now
        };

        await _databaseService.SaveVehicleAsync(vehicle);

        await LoadVehiclesAsync();

        ClearForm();
    }

    [RelayCommand]
    private void EditVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return;

        EditingVehicleId = vehicle.Id;

        FirstName = vehicle.FirstName;
        LastName = vehicle.LastName;

        PhoneNumber = vehicle.PhoneNumber;
        SmsNumber = vehicle.SmsNumber;
        EmailAddress = vehicle.EmailAddress;

        PlateNumber = vehicle.PlateNumber;
        VehicleType = vehicle.VehicleType;
        Color = vehicle.Color;

        VehicleDetails = vehicle.VehicleDetails;

        FormTitle = "Edit Vehicle";

        SaveButtonText = "Update Vehicle";
    }

    [RelayCommand]
    private async Task DeleteVehicleAsync(Vehicle vehicle)
    {
        if (vehicle == null)
            return;

        await _databaseService.DeleteVehicleAsync(vehicle);

        await LoadVehiclesAsync();
    }

    [RelayCommand]
    private void ClearForm()
    {
        EditingVehicleId = 0;

        FirstName = "";
        LastName = "";

        PhoneNumber = "";
        SmsNumber = "";
        EmailAddress = "";

        PlateNumber = "";
        VehicleType = "";
        Color = "";

        VehicleDetails = "";

        FormTitle = "Add Vehicle";

        SaveButtonText = "Add Vehicle";
    }
}