using SampleApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = SampleApp.Domain.Entities.Task;

namespace SampleApp.Infrastructure.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Task");

            builder.HasKey(ci => ci.Id);

            builder.Property(cb => cb.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cb => cb.Description)
                .HasMaxLength(250);

            builder.Property(cb => cb.DueDate)
                .IsRequired();

            builder.Property(cb => cb.StartDate)
                .IsRequired();

            builder.Property(cb => cb.EndDate)
                .IsRequired();

            builder.Property(cb => cb.Priority)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(cb => cb.Status)
                .IsRequired()
                .HasConversion<int>();
        }
    }
}
