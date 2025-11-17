using Domine.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoodBank.DTOs
{
    public sealed record IncomingTransferRequestApiDto(
    [Required] BankCode ExternalBankCode,
    [Required] string ExternalReference,
    [Required] string ToAccountCbu,
    [Required, Range(0.01, double.MaxValue)] decimal Amount,
    [Required] Currency Currency
);
}
