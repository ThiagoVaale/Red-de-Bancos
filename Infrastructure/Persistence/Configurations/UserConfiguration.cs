using Domine.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            var guidToString = new ValueConverter<Guid, string>(
                v => v.ToString(),
                v => Guid.Parse(v)
            );

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasConversion(guidToString)
                .IsRequired();

            b.Property(x => x.FullName)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .IsRequired();
        }
    }
}
