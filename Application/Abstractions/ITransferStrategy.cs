using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ITransferStrategy
    {
        Task<(bool success, string? externalReference, string? error)> SendAsync(ExternalTransferRequestDto request, CancellationToken ct);
    }
}
