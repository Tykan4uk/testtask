using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    internal class AuditoriumConfiguration : IEntityTypeConfiguration<Auditorium>
    {
        private readonly Auditorium[] _initialData =
        [
            new Auditorium
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Зал А",
                Capacity = 50,
                BaseRentalPrice = 2000
            },
            new Auditorium
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Зал В",
                Capacity = 100,
                BaseRentalPrice = 3500
            },
            new Auditorium
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Зал С",
                Capacity = 30,
                BaseRentalPrice = 1500
            }
        ];

        public void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.HasIndex(e => e.Name).IsUnique();

            builder.HasData(_initialData);
        }
    }
}