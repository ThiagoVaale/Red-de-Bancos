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

        public async Task<(bool success, string? externalReference, string? error)> SendAsync(
        ExternalTransferRequestDto request,CancellationToken ct)
        {
            var strategy = _resolver(request.ExternalBankCode);
            ExternalTransferResultDto result = await strategy.SendAsync(request, ct);
            return (
                success: result.IsSuccess,
                externalReference: result.ExternalReferenceId,
                error: result.ErrorMessage
            );
        }
    }
}
