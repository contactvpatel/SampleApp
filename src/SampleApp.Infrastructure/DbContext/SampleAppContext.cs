using SampleApp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SampleApp.Infrastructure.DbContext
{
    public class SampleAppContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SampleAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TaskConfiguration());
        }
    }
}
