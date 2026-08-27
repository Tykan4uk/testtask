using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Auditorium> Auditoriums { get; set; }

        public virtual DbSet<AuditoriumReserve> AuditoriumReserves { get; set; }

        public virtual DbSet<AuditoriumService> AuditoriumServices { get; set; }

        public virtual DbSet<AuditoriumReserveService> AuditoriumReserveServices { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<TimeRate> TimeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfiguration(new AuditoriumConfiguration());

            modelBuilder.ApplyConfiguration(new AuditoriumReserveConfiguration());

            modelBuilder.ApplyConfiguration(new AuditoriumReserveServiceConfiguration());

            modelBuilder.ApplyConfiguration(new AuditoriumServiceConfiguration());

            modelBuilder.ApplyConfiguration(new ServiceConfiguration());

            modelBuilder.ApplyConfiguration(new TimeRateConfiguration());
        }
    }
}
