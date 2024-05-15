using EventsApi.src.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.src.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Category);
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Creator);
        base.OnModelCreating(modelBuilder);
    }
}
