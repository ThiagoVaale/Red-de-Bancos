using Domine.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoodBank.DTOs
{
    public sealed record InternalTransferRequestApiDto(
    [Required] Guid FromAccountId,
    [Required] Guid ToAccountId,
    [Required, Range(0.01, double.MaxValue)] decimal Amount,
    [Required] Currency Currency
);
}
