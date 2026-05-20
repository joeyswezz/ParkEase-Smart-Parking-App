using Newtonsoft.Json;
using System.Text;
using ParkEase.Mobile.Models;
using ParkEase.Mobile.Data;

namespace ParkEase.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ParkingLotDatabaseService _parkingLotDb = new();
    private readonly VehicleDatabaseService _vehicleDb = new();
    private readonly BookingDatabaseService _bookingDb = new();

    private const string BaseUrl = "http://localhost:5201";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> AskParkingAssistantAsync(string question)
    {
        try
        {
            var parkingLots = await _parkingLotDb.GetParkingLotsAsync();
            var vehicles = await _vehicleDb.GetVehiclesAsync();
            var bookings = await _bookingDb.GetBookingsAsync();

            var request = new
            {
                userQuestion = question,

                parkingLotsJson = JsonConvert.SerializeObject(parkingLots.Select(l => new
                {
                    l.Name,
                    l.Location,
                    l.PricePerHour,
                    l.TotalSpaces,
                    l.AvailableSpaces,
                    l.SecurityLevel,
                    l.OpeningHours,
                    l.StatusLabel,
                    OccupancyPercentage = $"{l.OccupancyPercentage:F0}%"
                })),

                vehiclesJson = JsonConvert.SerializeObject(vehicles.Select(v => new
                {
                    v.FirstName,
                    v.LastName,
                    v.PlateNumber,
                    v.VehicleType,
                    v.Color
                })),

                bookingsJson = JsonConvert.SerializeObject(bookings.Select(b => new
                {
                    b.ParkingLotName,
                    b.VehiclePlateNumber,
                    b.BookingDate,
                    b.DurationHours,
                    b.TotalCost,
                    b.Status,
                    b.Notes
                }))
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/ai/parking-assistant", content);
            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result))
                return "No response was received from the AI service.";

            var aiResponse = JsonConvert.DeserializeObject<ParkingAssistantResponse>(result);

            if (aiResponse is null || string.IsNullOrWhiteSpace(aiResponse.Answer))
                return $"AI response was received but could not be read. Raw response: {result}";

            if (aiResponse.Answer.Contains("insufficient_quota", StringComparison.OrdinalIgnoreCase))
                return "ParkEase AI is connected, but the OpenAI account has no available quota or billing credit right now.";

            return aiResponse.Answer;
        }
        catch (Exception ex)
        {
            return $"AI assistant error: {ex.Message}";
        }
    }

    public async Task<List<ParkingLot>> GetParkingLotsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/parkinglots");

            if (!response.IsSuccessStatusCode)
                return new List<ParkingLot>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ParkingLot>>(json) ?? new List<ParkingLot>();
        }
        catch
        {
            return new List<ParkingLot>();
        }
    }

    public async Task<bool> AddParkingLotAsync(ParkingLot lot)
    {
        var json = JsonConvert.SerializeObject(lot);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BaseUrl}/parkinglots", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteParkingLotAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/parkinglots/{id}");
        return response.IsSuccessStatusCode;
    }
}