using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseManagementAPI.Data.Entities;


namespace CourseManagementAPI.Infrastructure.Configurations

{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);
            builder.Property(c => c.CourseId)
                .IsRequired()
                .HasMaxLength(26) // ULID length
                .ValueGeneratedNever(); // ULID is generated in the entity
            builder.Property(c => c.TrainerId)
                .IsRequired()
                .HasMaxLength(26); // ULID length
            
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.Price) 
                .HasPrecision(18, 2); 
            builder.HasOne(c => c.Trainer)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TrainerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasIndex(c => c.Title);
            builder.HasIndex(c => c.Description);
        }
    }
}