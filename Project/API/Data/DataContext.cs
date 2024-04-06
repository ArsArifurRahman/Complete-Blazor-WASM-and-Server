using API.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : IdentityDbContext<ApplicationUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; } = null!;
    public virtual DbSet<Book> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.Biography).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA09FAB742")
                .IsUnique();

            entity.Property(e => e.Image)
            .HasMaxLength(50);

            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");

            entity.Property(e => e.Price)
            .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Summary)
            .HasMaxLength(250);

            entity.Property(e => e.Title)
            .HasMaxLength(50);

            entity.HasOne(d => d.Author)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_ToTable");
        });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "Administrator".ToUpper(),
                Id = "070a9777-88b4-49c6-bf12-12e2fbc3817f",
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "User".ToUpper(),
                Id = "bafdf2bf-c6d1-4064-a431-df82f89e3fdd",
            }
        );

        var hasher = new PasswordHasher<ApplicationUser>();

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "f9a88723-6704-49a0-b1c1-730927840c45",
                Email = "admin@bookstore.com",
                NormalizedEmail = "admin@bookstore.com".ToUpper(),
                UserName = "admin@bookstore.com",
                NormalizedUserName = "admin@bookstore.com".ToUpper(),
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = hasher.HashPassword(null, "p@55w0rd1")
            },
            new ApplicationUser
            {
                Id = "41a921d4-44a9-48e1-8482-8816d20b0281",
                Email = "user@bookstore.com",
                NormalizedEmail = "user@bookstore.com".ToUpper(),
                UserName = "user@bookstore.com",
                NormalizedUserName = "user@bookstore.com".ToUpper(),
                FirstName = "System",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null, "p@55w0rd1")
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                // Admin Role
                RoleId = "070a9777-88b4-49c6-bf12-12e2fbc3817f",
                UserId = "f9a88723-6704-49a0-b1c1-730927840c45"
            },
            new IdentityUserRole<string>
            {
                // User Role
                RoleId = "bafdf2bf-c6d1-4064-a431-df82f89e3fdd",
                UserId = "41a921d4-44a9-48e1-8482-8816d20b0281"
            }
        );
    }
}
