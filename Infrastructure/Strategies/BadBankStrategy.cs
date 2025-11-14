using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategies
{
    public sealed class BadBankStrategy : ITransferStrategy
    {
        public async Task<(bool success, string? externalReference, string? error)> SendAsync(
            ExternalTransferRequestDto request,
            CancellationToken ct)
        {
            await Task.Delay(100, ct); 
            return (true, $"BAD1-{Guid.NewGuid():N}", null);
        }
    }
}
