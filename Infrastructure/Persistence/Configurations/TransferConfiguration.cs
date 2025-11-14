using Domine.Entities;
using Domine.Enums;
using Domine.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        
        private static string MoneyToString(Money m)
            => $"{m.Amount}|{m.Currency}";

        private static Money StringToMoney(string s)
        {
            var parts = s.Split('|');
            var amount = decimal.Parse(parts[0]);
            var currency = Enum.Parse<Currency>(parts[1]);
            return new Money(amount, currency);
        }

        public void Configure(EntityTypeBuilder<Transfer> b)
        {
            
            var guidConv = new ValueConverter<Guid, string>(
                v => v.ToString(),
                v => Guid.Parse(v)
            );

            var nullableGuidConv = new ValueConverter<Guid?, string?>(
                v => v.HasValue ? v.Value.ToString() : null,
                v => v != null ? Guid.Parse(v) : (Guid?)null
            );

            
            var statusConv = new ValueConverter<TransferStatus, string>(
                v => v.ToString(),
                v => Enum.Parse<TransferStatus>(v)
            );

            var directionConv = new ValueConverter<TransferDirection, string>(
            
            var bankConv = new ValueConverter<BankCode?, string?>(
                v => v.HasValue ? v.Value.ToString() : null,
                v => v != null ? Enum.Parse<BankCode>(v) : (BankCode?)null
            );
                b.HasKey(x => x.Id);

                b.Property(x => x.Id)
                    .HasConversion(guidConv)
                    .IsRequired();

                b.Property(x => x.FromAccountId)
                    .HasConversion(nullableGuidConv);

                b.Property(x => x.ToAccountId)
                    .HasConversion(nullableGuidConv);

                b.Property(x => x.Amount)
                    .HasConversion(moneyConv)
                    .HasColumnType("TEXT")
                    .IsRequired();

                b.Property(x => x.Status)
                    .HasConversion(statusConv)
                    .IsRequired();

                b.Property(x => x.Direction)
                    .HasConversion(directionConv)
                    .IsRequired();

                b.Property(x => x.ExternalBankCode)
                    .HasConversion(bankConv);

                b.Property(x => x.ExternalReference)
                    .HasMaxLength(200);

                b.Property(x => x.Reason)
                    .HasMaxLength(300);

                b.Property(x => x.CreatedAt)
                    .IsRequired();

                b.Property(x => x.CompletedAt);

                b.Property(x => x.CanceledAt);
                .HasMaxLength(200);

            b.Property(x => x.Reason)
                .HasMaxLength(300);

            b.Property(x => x.CreatedAt)
                .IsRequired();
            
            b.Property(x => x.CanceledAt);
        }
    }
}

            