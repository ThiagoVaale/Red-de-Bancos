using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Gateways
{
    public sealed class ExternalTransferGateway : IExternalTransferGateway
    {
        private readonly StrategyResolver _resolver;

        public ExternalTransferGateway(StrategyResolver resolver)
        {
            _resolver = resolver;
        }

        public Task<(bool success, string? externalReference, string? error)> SendAsync(
        ExternalTransferRequestDto request,CancellationToken ct)
        {
            var strategy = _resolver(request.ExternalBankCode);
            return strategy.SendAsync(request, ct);
        }
    }
}
