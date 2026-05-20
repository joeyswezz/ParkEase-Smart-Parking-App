using Microsoft.EntityFrameworkCore;
using ParkEase.Api.Models;

namespace ParkEase.Api.Data;

public class ParkEaseDb : DbContext
{
    public ParkEaseDb(DbContextOptions<ParkEaseDb> options) : base(options)
    {
    }

    public DbSet<ParkingLot> ParkingLots => Set<ParkingLot>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Booking> Bookings => Set<Booking>();
}