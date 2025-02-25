using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Infrastructure.Context;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    private readonly ILogger<ApplicationDbContext> _logger;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
        : base(options)
    {
        _logger = logger;
        
    }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation("Configuring database model");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new TrainerConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        // Seed Roles with specific IDs to prevent duplicates
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "01HMYX5Z7K8P9Q2W3R4T5V6J",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "01HMYX5Z7K8P9Q2W3R4T5V6K"
            },
            new IdentityRole
            {
                Id = "01HMYX5Z7K8P9Q2W3R4T5V7M",
                Name = "Trainer",
                NormalizedName = "TRAINER",
                ConcurrencyStamp = "01HMYX5Z7K8P9Q2W3R4T5V7N"
            }
        );
        _logger.LogInformation("Database model configuration completed");
    }
}