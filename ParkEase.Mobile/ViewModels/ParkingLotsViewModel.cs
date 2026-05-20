using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class ParkingLotsViewModel : ObservableObject
{
    private readonly ParkingLotDatabaseService _parkingLotDb = new();

    public ObservableCollection<ParkingLot> ParkingLots { get; } = new();

    [ObservableProperty]
    private bool isBusy;

    public ParkingLotsViewModel()
    {
        _ = LoadParkingLotsAsync();
    }

    [RelayCommand]
    public async Task LoadParkingLotsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ParkingLots.Clear();

            var lots = await _parkingLotDb.GetParkingLotsAsync();

            foreach (var lot in lots)
                ParkingLots.Add(lot);
        }
        finally
        {
            IsBusy = false;
        }
    }
}