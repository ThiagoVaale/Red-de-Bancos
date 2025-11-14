using Domine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed record InternalTransferRequestDto(
        Guid FromAccountId,
        Guid ToAccountId,
        decimal Amount,
        Currency Currency
    );
}
