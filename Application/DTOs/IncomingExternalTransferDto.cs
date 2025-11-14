using Domine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed record IncomingExternalTransferDto(
        BankCode ExternalBankCode,
        string ExternalReference, 
        string ToAccountCbu,
        decimal Amount,
        Currency Currency
    );
}
