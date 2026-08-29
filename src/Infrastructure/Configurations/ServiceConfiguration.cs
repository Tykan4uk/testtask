using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        private readonly Service[] _initialData =
        [
            new Service {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Проєктор",
                Price = 500
            },
            new Service {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Wi-Fi",
                Price = 300
            },
            new Service {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Звук",
                Price = 700
            }
        ];

        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.HasIndex(e => e.Name).IsUnique();

            builder.HasData(_initialData);
        }
    }
}