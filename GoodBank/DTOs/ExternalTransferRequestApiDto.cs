using Domine.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoodBank.DTOs
{
    public sealed record ExternalTransferRequestApiDto(
     [Required] Guid FromAccountId,
     [Required] BankCode ExternalBankCode,
     string? ExternalAccountRef,
     [Required, Range(0.01, double.MaxValue)] decimal Amount,
     [Required] Currency Currency
    );
}
