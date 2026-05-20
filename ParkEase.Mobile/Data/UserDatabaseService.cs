using SQLite;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.Data;

public class UserDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public UserDatabaseService()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "parkease.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<User>().Wait();
    }

    public async Task<int> RegisterUserAsync(User user)
    {
        return await _database.InsertAsync(user);
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        return await _database.Table<User>()
            .Where(u => u.EmailAddress == email && u.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _database.Table<User>()
            .Where(u => u.EmailAddress == email)
            .FirstOrDefaultAsync();
    }
}