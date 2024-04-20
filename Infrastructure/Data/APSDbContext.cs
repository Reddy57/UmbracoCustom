using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;


public class AfsDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public AfsDbContext(DbContextOptions<AfsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(ConfigureUser);
        modelBuilder.Entity<Role>(ConfigureRole);
        modelBuilder.Entity<UserRole>(ConfigureUserRole);

        // Configure the schema names for the Identity tables
        modelBuilder.Entity<IdentityUserClaim<int>>(uc => uc.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<int>>(ul => ul.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<int>>(ut => ut.ToTable("UserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<int>>(rc => rc.ToTable("RoleClaims"));
    }

    private void ConfigureUserRole(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        // If UserRole has a composite key, define it here if needed
        // e.g., builder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }

    private void ConfigureRole(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(64);

        // Navigation properties and foreign keys
        builder.HasMany(e => e.UsersForRole)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
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
