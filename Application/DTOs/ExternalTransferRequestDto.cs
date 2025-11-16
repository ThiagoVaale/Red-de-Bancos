using Domine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed record ExternalTransferRequestDto(
        Guid transferId,
        Guid FromAccountId,
        BankCode ExternalBankCode,
        string? ExternalAccountRef, 
        decimal Amount,
        Currency Currency
    );
}
