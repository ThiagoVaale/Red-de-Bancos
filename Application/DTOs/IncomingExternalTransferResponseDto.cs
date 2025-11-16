using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed record IncomingExternalTransferResponseDto(
        bool Success,
        string Message,
        Guid? TransferId
    );
}

