using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    internal class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        private readonly UserInfo[] _initialData =
        [
            new UserInfo
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserName = "admin",
                Email = "admin@email.com",
                Password = "473287F8298DBA7163A897908958F7C0EAE733E25D2E027992EA2EDC9BED2FA8"
            }
        ];

        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.UserName).HasMaxLength(100);

            builder.Property(e => e.Email).HasMaxLength(100);

            builder.HasIndex(e => e.Email).IsUnique();

            builder.HasData(_initialData);
        }
    }
}