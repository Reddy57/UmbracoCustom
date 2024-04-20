using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;


public class AfsDbContext : IdentityDbContext<AfsUser, AfsRole, int, IdentityUserClaim<int>, AfsUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public AfsDbContext(DbContextOptions<AfsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AfsUser>(ConfigureUser);
        modelBuilder.Entity<AfsRole>(ConfigureRole);
        modelBuilder.Entity<AfsUserRole>(ConfigureUserRole);

        // Configure the schema names for the Identity tables
        modelBuilder.Entity<IdentityUserClaim<int>>(uc => uc.ToTable("AfsUserClaims"));
        modelBuilder.Entity<IdentityUserLogin<int>>(ul => ul.ToTable("AfsUserLogins"));
        modelBuilder.Entity<IdentityUserToken<int>>(ut => ut.ToTable("AfsUserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<int>>(rc => rc.ToTable("AfsRoleClaims"));
    }

    private void ConfigureUserRole(EntityTypeBuilder<AfsUserRole> builder)
    {
        builder.ToTable("AfsUserRoles");
        // If UserRole has a composite key, define it here if needed
        // e.g., builder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }

    private void ConfigureRole(EntityTypeBuilder<AfsRole> builder)
    {
        builder.ToTable("AfsRoles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(64);

        // Navigation properties and foreign keys
        builder.HasMany(e => e.UsersForRole)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }

    private void ConfigureUser(EntityTypeBuilder<AfsUser> builder)
    {
        builder.ToTable("AfsUsers");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).HasMaxLength(128);
        builder.Property(u => u.LastName).HasMaxLength(128);

        // Navigation properties and foreign keys
        builder.HasMany(e => e.RolesForUser)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}
