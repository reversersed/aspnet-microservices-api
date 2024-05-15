using Microsoft.EntityFrameworkCore;
using NewsApi.src.Data.Entities;

namespace NewsApi.src.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
            entity.HasMany(x => x.Comments)
                  .WithOne(x => x.Article)
                  .OnDelete(DeleteBehavior.Cascade)
        );
        base.OnModelCreating(modelBuilder);
    }
}