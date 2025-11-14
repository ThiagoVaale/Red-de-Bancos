using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategies
{
    public sealed class WorstBankStrategy : ITransferStrategy
    {
        public async Task<(bool success, string? externalReference, string? error)> SendAsync(
        ExternalTransferRequestDto request,
        CancellationToken ct)
        {
            await Task.Delay(150, ct);
            bool ok = DateTime.UtcNow.Millisecond % 3 != 0;
            return ok ? (true, $"BAD3-{Guid.NewGuid():N}", null) : (false, null, "Upstream error");
        }
    }
}
