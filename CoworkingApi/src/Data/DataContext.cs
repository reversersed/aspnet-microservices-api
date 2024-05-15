using CoworkingApi.src.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoworkingApi.src.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Holder> Holders {  get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Holder);
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room);
        base.OnModelCreating(modelBuilder);
    }
}