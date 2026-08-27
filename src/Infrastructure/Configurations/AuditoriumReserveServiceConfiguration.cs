using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class AuditoriumReserveServiceConfiguration : IEntityTypeConfiguration<AuditoriumReserveService>
    {
        public void Configure(EntityTypeBuilder<AuditoriumReserveService> builder)
        {
            builder.HasKey(x => new
            {
                x.AuditoriumReserveId,
                x.AuditoriumServiceId
            });

            builder.HasOne(x => x.AuditoriumReserve)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.AuditoriumReserveId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AuditoriumService)
                .WithMany()
                .HasForeignKey(x => x.AuditoriumServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
