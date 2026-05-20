using SQLite;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public class BookingDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public BookingDatabaseService()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "parkease.db3");

        _database = new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<Booking>().Wait();
    }

    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await _database.Table<Booking>()
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<int> SaveBookingAsync(Booking booking)
    {
        if (booking.Id != 0)
            return await _database.UpdateAsync(booking);

        return await _database.InsertAsync(booking);
    }

    public async Task<int> DeleteBookingAsync(Booking booking)
    {
        return await _database.DeleteAsync(booking);
    }
}