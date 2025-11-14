using Domine.Entities;
using Domine.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> b)
        {
            var guidToString = new ValueConverter<Guid, string>(
                v => v.ToString(),
                v => Guid.Parse(v)
            );

            var currencyToString = new ValueConverter<Currency, string>(
                v => v.ToString(),
                v => Enum.Parse<Currency>(v)
            );

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasConversion(guidToString)
                .IsRequired();

            b.Property(x => x.UserId)
                .HasConversion(guidToString)
                .IsRequired();

            b.Property(x => x.Currency)
                .HasConversion(currencyToString)
                .IsRequired();

            b.Property(x => x.Cbu)
                .HasMaxLength(50)
                .IsRequired();

            b.Property(x => x.Balance)
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.IsActive)
                .IsRequired();

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
