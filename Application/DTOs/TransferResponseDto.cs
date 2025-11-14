using Domine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed record TransferResponseDto(
        Guid TransferId,
        TransferStatus Status,
        TransferDirection Direction,
        decimal Amount,
        Currency Currency,
        DateTime CreatedAt,
        string? ExternalReference
    );
}
