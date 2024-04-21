using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class AfsDbContext : DbContext
{
    public AfsDbContext(DbContextOptions<AfsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<UserCredentials> UserCredentials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(ConfigureUser);
        modelBuilder.Entity<UserCredentials>(ConfigureUserCredentials);
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureUserCredentials(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.HasIndex(uc => uc.UserId).IsUnique();
        builder.Property(uc => uc.Password).HasMaxLength(256);
        builder.Property(uc => uc.Salt).HasMaxLength(256);
    }


    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.FirstName).HasMaxLength(128);
        builder.Property(u => u.LastName).HasMaxLength(128);
    }
}