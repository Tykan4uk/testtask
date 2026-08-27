using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class TimeRateConfiguration : IEntityTypeConfiguration<TimeRate>
    {
        private readonly TimeRate[] _initialData =
        [
            new TimeRate {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                StartTime = TimeOnly.Parse("09:00:00"),
                EndTime = TimeOnly.Parse("17:59:59"),
                Rate = 1
            },
            new TimeRate {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                StartTime = TimeOnly.Parse("18:00:00"),
                EndTime = TimeOnly.Parse("22:59:59"),
                Rate = 0.8m
            },
            new TimeRate {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                StartTime = TimeOnly.Parse("06:00:00"),
                EndTime = TimeOnly.Parse("08:59:59"),
                Rate = 0.9m
            },
            new TimeRate {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                StartTime = TimeOnly.Parse("12:00:00"),
                EndTime = TimeOnly.Parse("13:59:59"),
                Rate = 1.15m
            }
        ];

        public void Configure(EntityTypeBuilder<TimeRate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(_initialData);
        }
    }
}
