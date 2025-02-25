using CourseManagementAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManagementAPI.Infrastructure.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(t => t.TrainerId);
        builder.Property(t => t.TrainerId)
            .IsRequired()
            .HasMaxLength(26) // ULID length
            .ValueGeneratedNever(); // ULID is generated in the entity
        
        builder.Property(t => t.ApplicationUserId)
            .IsRequired()
            .HasMaxLength(26); // ULID length
        
        builder.Property(t => t.Bio).HasMaxLength(500);

        builder.HasOne(t => t.ApplicationUser)
            .WithOne(u => u.Trainer)
            .HasForeignKey<Trainer>(t => t.ApplicationUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Courses)
            .WithOne(c => c.Trainer)
            .HasForeignKey(c => c.TrainerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(t => t.Payments)
            .WithOne(p => p.Trainer)
            .HasForeignKey(p => p.TrainerId)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}