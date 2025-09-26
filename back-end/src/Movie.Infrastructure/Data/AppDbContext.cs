using Movie.Domain.Entities;

namespace Movie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext
{
    public DbSet<WatchHistory> WatchHistories { get; set; } = null!;
    public DbSet<AppUser> AppUsers { get; set; } =null!;
    public DbSet<MovieItem> MovieItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieItem>()
            .HasIndex(x=> x.Slug).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}