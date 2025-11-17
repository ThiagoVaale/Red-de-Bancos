using Application.Abstractions;
using Application.DTOs;
using Domine.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategies
{
    public sealed class WorseBankStrategy : IExternalTransferStrategy
    {
            private readonly ILogger<WorseBankStrategy> _logger;
            private static readonly Random _random = new();

            public WorseBankStrategy(ILogger<WorseBankStrategy> logger)
            {
                _logger = logger;
            }

            public BankCode SupportedBank => BankCode.WorseBank;

            public async Task<ExternalTransferResultDto> SendAsync(
                ExternalTransferRequestDto transfer,
                CancellationToken ct)
            {
                _logger.LogInformation("WorseBank: Procesando transferencia simulada...");

                try
                {
                    var delay = _random.Next(100, 1500);
                    await Task.Delay(delay, ct);

                    var chance = _random.NextDouble();

                    if (chance < 0.6)
                    {
                        var externalRef = $"WORSE-{Guid.NewGuid():N}";
                        _logger.LogInformation("WorseBank: Éxito simulado. Ref: {ref}", externalRef);

                        return ExternalTransferResultDto.Success(externalRef);
                    }

                    if (chance < 0.8)
                    {
                        _logger.LogWarning("WorseBank: Error transitorio simulado.");
                        return ExternalTransferResultDto.Failure("WorseBank transient error.", isTransient: true);
                    }

                    _logger.LogError("WorseBank: Error permanente simulado.");
                    return ExternalTransferResultDto.Failure("WorseBank permanent failure.");
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    _logger.LogWarning("WorseBank transferencia cancelada por el llamador.");
                    return ExternalTransferResultDto.Failure("Transfer cancelled by caller.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "WorseBank: Error inesperado.");
                    return ExternalTransferResultDto.Failure("Unexpected error contacting WorseBank.");
                }
            }
        }
}
