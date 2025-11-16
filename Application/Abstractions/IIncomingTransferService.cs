using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IIncomingTransferService
    {
        Task<Guid> ProcessIncomingTransfer(IncomingExternalTransferDto dto, CancellationToken ct);
    }
}
