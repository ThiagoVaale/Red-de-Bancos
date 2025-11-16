using Application.DTOs;
using Domine.Entities;
using Domine.Enums;
using GoodBank.Application.Interfaces;
using GoodBank.Application.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Strategies;

public class WorstBankStrategy : IExternalTransferStrategy
{
    private readonly ILogger<WorstBankStrategy> _logger;

    public WorstBankStrategy(ILogger<WorstBankStrategy> logger)
    {
        _logger = logger;
    }

    public BankCode SupportedBank => BankCode.WorstBank;

    public async Task<ExternalTransferResult> SendAsync(ExternalTransferRequestDto transfer, CancellationToken ct)
    {
        _logger.LogWarning("Intentando conectar con WorstBank para {TransferId}...", transfer.transferId);

        try
        {
            // 1. Simular latencia muy alta o timeout
            var delay = Random.Shared.Next(1500, 5000); // Tarda entre 1.5 y 5 segundos

            // Si el delay simulado es > 3s, simulamos un timeout real
            // El CancellationToken (ct) que viene del servicio (o de Polly) se activaría.
            if (delay > 3000)
            {
                _logger.LogError("WorstBank está tardando demasiado (simulando timeout).");
                await Task.Delay(TimeSpan.FromSeconds(10), ct); // Esto forzará el TaskCanceledException
            }

            // Si no es un timeout, es un delay normal
            await Task.Delay(delay, ct);

            // 2. Simular éxito o fracaso (solo 10% de éxito)
            if (Random.Shared.Next(0, 10) == 0)
            {
                var externalRef = $"WORST_{Guid.NewGuid().ToString("N")[..8]}";
                _logger.LogInformation("Transferencia a WorstBank (milagrosamente) exitosa. Ref: {Ref}", externalRef);
                return ExternalTransferResult.Success(externalRef);
            }
            else
            {
                _logger.LogError("Transferencia a WorstBank fallida (simulado).");
                return ExternalTransferResult.Failure("WorstBank: Error 503 Service Unavailable.");
            }
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "¡Timeout confirmado! WorstBank no respondió para la transferencia {Id}", transfer.transferId);
            return ExternalTransferResult.Failure("Timeout: El servicio de WorstBank no está disponible.");
        }
    }
}