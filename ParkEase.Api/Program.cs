using Microsoft.EntityFrameworkCore;
using ParkEase.Api.Data;
using ParkEase.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ParkEaseDb>(options =>
    options.UseSqlite("Data Source=parkease.db"));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "ParkEase API is running");

// Auto-create database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParkEaseDb>();
    db.Database.EnsureCreated();

    if (!db.ParkingLots.Any())
    {
        db.ParkingLots.AddRange(
            new ParkingLot
            {
                Name = "New Kingston Secure Parking",
                Address = "New Kingston, Jamaica",
                PricePerHour = 250,
                TotalSpaces = 80,
                AvailableSpaces = 35,
                SecurityLevel = "High",
                OpeningHours = "24 Hours",
                ImageUrl = "parking1.png"
            },
            new ParkingLot
            {
                Name = "Half Way Tree Plaza Parking",
                Address = "Half Way Tree, Jamaica",
                PricePerHour = 180,
                TotalSpaces = 60,
                AvailableSpaces = 22,
                SecurityLevel = "Medium",
                OpeningHours = "7:00 AM - 9:00 PM",
                ImageUrl = "parking2.png"
            }
        );

        db.SaveChanges();
    }
}

// PARKING LOT CRUD
app.MapGet("/parkinglots", async (ParkEaseDb db) =>
    await db.ParkingLots.ToListAsync());

app.MapGet("/parkinglots/{id:int}", async (int id, ParkEaseDb db) =>
    await db.ParkingLots.FindAsync(id) is ParkingLot lot
        ? Results.Ok(lot)
        : Results.NotFound());

app.MapPost("/parkinglots", async (ParkingLot lot, ParkEaseDb db) =>
{
    db.ParkingLots.Add(lot);
    await db.SaveChangesAsync();
    return Results.Created($"/parkinglots/{lot.Id}", lot);
});

app.MapPut("/parkinglots/{id:int}", async (int id, ParkingLot input, ParkEaseDb db) =>
{
    var lot = await db.ParkingLots.FindAsync(id);

    if (lot is null)
        return Results.NotFound();

    lot.Name = input.Name;
    lot.Address = input.Address;
    lot.PricePerHour = input.PricePerHour;
    lot.TotalSpaces = input.TotalSpaces;
    lot.AvailableSpaces = input.AvailableSpaces;
    lot.SecurityLevel = input.SecurityLevel;
    lot.OpeningHours = input.OpeningHours;
    lot.ImageUrl = input.ImageUrl;

    await db.SaveChangesAsync();

    return Results.Ok(lot);
});

app.MapDelete("/parkinglots/{id:int}", async (int id, ParkEaseDb db) =>
{
    var lot = await db.ParkingLots.FindAsync(id);

    if (lot is null)
        return Results.NotFound();

    db.ParkingLots.Remove(lot);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// VEHICLE CRUD
app.MapGet("/vehicles", async (ParkEaseDb db) =>
    await db.Vehicles.ToListAsync());

app.MapPost("/vehicles", async (Vehicle vehicle, ParkEaseDb db) =>
{
    db.Vehicles.Add(vehicle);
    await db.SaveChangesAsync();
    return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
});

app.MapPut("/vehicles/{id:int}", async (int id, Vehicle input, ParkEaseDb db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);

    if (vehicle is null)
        return Results.NotFound();

    vehicle.OwnerName = input.OwnerName;
    vehicle.PlateNumber = input.PlateNumber;
    vehicle.VehicleType = input.VehicleType;
    vehicle.Color = input.Color;

    await db.SaveChangesAsync();

    return Results.Ok(vehicle);
});

app.MapDelete("/vehicles/{id:int}", async (int id, ParkEaseDb db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);

    if (vehicle is null)
        return Results.NotFound();

    db.Vehicles.Remove(vehicle);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// BOOKING CRUD
app.MapGet("/bookings", async (ParkEaseDb db) =>
    await db.Bookings.ToListAsync());

app.MapPost("/bookings", async (Booking booking, ParkEaseDb db) =>
{
    var lot = await db.ParkingLots.FindAsync(booking.ParkingLotId);

    if (lot is null)
        return Results.BadRequest("Parking lot not found.");

    if (lot.AvailableSpaces <= 0)
        return Results.BadRequest("No spaces available.");

    booking.TotalCost = lot.PricePerHour * booking.DurationHours;
    booking.Status = "Active";

    lot.AvailableSpaces--;

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Created($"/bookings/{booking.Id}", booking);
});

app.MapPut("/bookings/{id:int}/cancel", async (int id, ParkEaseDb db) =>
{
    var booking = await db.Bookings.FindAsync(id);

    if (booking is null)
        return Results.NotFound();

    if (booking.Status == "Cancelled")
        return Results.BadRequest("Booking is already cancelled.");

    booking.Status = "Cancelled";

    var lot = await db.ParkingLots.FindAsync(booking.ParkingLotId);

    if (lot is not null)
        lot.AvailableSpaces++;

    await db.SaveChangesAsync();

    return Results.Ok(booking);
});

app.MapDelete("/bookings/{id:int}", async (int id, ParkEaseDb db) =>
{
    var booking = await db.Bookings.FindAsync(id);

    if (booking is null)
        return Results.NotFound();

    db.Bookings.Remove(booking);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// AI PARKING ASSISTANT
app.MapPost("/ai/parking-assistant", async (ParkingAssistantRequest request, IConfiguration config) =>
{
    var apiKey = config["OpenAI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Results.BadRequest(new
        {
            answer = "OpenAI API key is missing."
        });
    }

    using var client = new HttpClient();

    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

    var prompt = $"""
    You are ParkEase AI, a helpful parking assistant for a mobile parking reservation app.

    Help the user choose the best parking option based on:
    - price
    - available spaces
    - security level
    - opening hours
    - location
    - vehicle information
    - existing bookings

    Keep the answer short, friendly, and practical.

    User question:
    {request.UserQuestion}

    Parking lots:
    {request.ParkingLotsJson}

    Vehicles:
    {request.VehiclesJson}

    Bookings:
    {request.BookingsJson}
    """;

    var body = new
    {
        model = "gpt-4.1-mini",
        input = prompt
    };

    var response = await client.PostAsJsonAsync(
        "https://api.openai.com/v1/responses",
        body);

    var json = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        return Results.BadRequest(new
        {
            answer = $"AI request failed: {json}"
        });
    }

    using var doc = System.Text.Json.JsonDocument.Parse(json);

    string answer = "";

    if (doc.RootElement.TryGetProperty("output_text", out var outputText))
    {
        answer = outputText.GetString() ?? "";
    }
    else
    {
        answer = "AI response received, but no readable answer was found.";
    }

    return Results.Ok(new
    {
        answer
    });
});

app.Run();