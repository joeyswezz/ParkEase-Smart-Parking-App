using SQLite;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public class PaymentDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public PaymentDatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "parkease.db");

        _database = new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<PaymentCard>().Wait();
    }

    public Task<List<PaymentCard>> GetCardsAsync()
    {
        return _database.Table<PaymentCard>().ToListAsync();
    }

    public Task<int> SaveCardAsync(PaymentCard card)
    {
        if (card.Id != 0)
            return _database.UpdateAsync(card);

        return _database.InsertAsync(card);
    }

    public Task<int> DeleteCardAsync(PaymentCard card)
    {
        return _database.DeleteAsync(card);
    }
}