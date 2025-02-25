
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseManagementAPI.Data.Entities;

namespace CourseManagementAPI.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.PaymentId);
        builder.Property(p => p.PaymentId)
            .IsRequired()
            .HasMaxLength(26) // ULID length
            .ValueGeneratedNever(); // ULID is generated in the entity
        
        builder.Property(p => p.TrainerId)
            .IsRequired()
            .HasMaxLength(26); // ULID length
        
        builder.Property(p => p.CourseId)
            .IsRequired()
            .HasMaxLength(26); // ULID length
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.PaymentDate).IsRequired();

        builder.HasOne(p => p.Trainer)
            .WithMany(t => t.Payments)
            .HasForeignKey(p => p.TrainerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Course)
            .WithMany()
            .HasForeignKey(p => p.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

    }
}