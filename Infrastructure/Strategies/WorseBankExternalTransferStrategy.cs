using Application.DTOs;
using Domine.Entities;
using Domine.Enums;
using GoodBank.Application.Interfaces;
using GoodBank.Application.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Strategies;

public class WorseBankStrategy : IExternalTransferStrategy
{
    private readonly ILogger<WorseBankStrategy> _logger;

    public WorseBankStrategy(ILogger<WorseBankStrategy> logger)
    {
        _logger = logger;
    }

    public BankCode SupportedBank => BankCode.WorseBank;

    public async Task<ExternalTransferResult> SendAsync(ExternalTransferRequestDto transfer, CancellationToken ct)
    {
        _logger.LogInformation("Iniciando transferencia a WorseBank para {TransferId}...", transfer.transferId);

        try
        {
            // Simular latencia de red variable
            await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(200, 700)), ct);

            // 1. Simular éxito o fracaso (60% de éxito)
            if (Random.Shared.Next(0, 10) < 6)
            {
                // ÉXITO
                var externalRef = $"WORSE_{Guid.NewGuid().ToString("N")[..10]}";
                _logger.LogInformation("Transferencia a WorseBank exitosa. RefExterna: {ExternalRef}", externalRef);
                return ExternalTransferResult.Success(externalRef);
            }
            else
            {
                // ERROR SIMULADO
                _logger.LogWarning("Transferencia a WorseBank {TransferId} fallida (error simulado).", transfer.transferId);
                return ExternalTransferResult.Failure("WorseBank: Conexión rechazada por el host.");
            }
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "La transferencia a WorseBank fue cancelada (timeout).");
            return ExternalTransferResult.Failure("Timeout: WorseBank no respondió a tiempo.");
        }
    }
}