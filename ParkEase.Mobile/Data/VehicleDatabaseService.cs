using SQLite;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public class VehicleDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public VehicleDatabaseService()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "parkease.db3");

        _database = new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<Vehicle>().Wait();
    }

    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        return await _database.Table<Vehicle>().ToListAsync();
    }

    public async Task<int> SaveVehicleAsync(Vehicle vehicle)
    {
        if (vehicle.Id != 0)
            return await _database.UpdateAsync(vehicle);

        return await _database.InsertAsync(vehicle);
    }

    public async Task<int> DeleteVehicleAsync(Vehicle vehicle)
    {
        return await _database.DeleteAsync(vehicle);
    }
}