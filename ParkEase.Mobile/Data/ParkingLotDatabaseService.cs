using SQLite;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public class ParkingLotDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public ParkingLotDatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "parkease.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<ParkingLot>().Wait();
    }

    public Task<List<ParkingLot>> GetParkingLotsAsync()
    {
        return _database.Table<ParkingLot>().ToListAsync();
    }

    public Task<int> SaveParkingLotAsync(ParkingLot lot)
    {
        if (lot.Id != 0)
            return _database.UpdateAsync(lot);

        return _database.InsertAsync(lot);
    }

    public Task<int> DeleteParkingLotAsync(ParkingLot lot)
    {
        return _database.DeleteAsync(lot);
    }

    public async Task ResetParkingLotsTableAsync()
    {
        await _database.DropTableAsync<ParkingLot>();
        await _database.CreateTableAsync<ParkingLot>();
    }
}