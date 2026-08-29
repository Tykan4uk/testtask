using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class AuditoriumServiceConfiguration : IEntityTypeConfiguration<AuditoriumService>
    {
        public void Configure(EntityTypeBuilder<AuditoriumService> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new
                {
                    x.AuditoriumId,
                    x.ServiceId
                })
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Auditorium)
                .WithMany(x => x.AuditoriumServices)
                .HasForeignKey(x => x.AuditoriumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
