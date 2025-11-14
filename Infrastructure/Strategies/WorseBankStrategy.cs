using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategies
{
    public sealed class WorseBankStrategy : ITransferStrategy
    {
        public async Task<(bool success, string? externalReference, string? error)> SendAsync(
        ExternalTransferRequestDto request,
        CancellationToken ct)
        {
            await Task.Delay(120, ct);
            return (true, $"BAD2-{Guid.NewGuid():N}", null);
        }
    }
}
