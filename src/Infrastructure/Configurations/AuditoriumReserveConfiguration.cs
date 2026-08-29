using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class AuditoriumReserveConfiguration : IEntityTypeConfiguration<AuditoriumReserve>
    {
        public void Configure(EntityTypeBuilder<AuditoriumReserve> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}