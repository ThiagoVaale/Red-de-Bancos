using Application.Abstractions;
using Application.DTOs;
using GoodBank.Application.Models;
using System.Threading;
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
        ExternalTransferRequestDto request, CancellationToken ct)
        {
            var strategy = _resolver(request.ExternalBankCode);

            // AWAIT la llamada para obtener el objeto ExternalTransferResult
            ExternalTransferResult result = await strategy.SendAsync(request, ct);

            // 🚨 Mapeo Corregido: Usando IsSuccess y ExternalReferenceId
            return (
                // La tupla espera 'success' (bool), se mapea desde IsSuccess
                success: result.IsSuccess,
                // La tupla espera 'externalReference' (string?), se mapea desde ExternalReferenceId
                externalReference: result.ExternalReferenceId,
                // La tupla espera 'error' (string?), se mapea desde ErrorMessage
                error: result.ErrorMessage
            );
        }
    }
}