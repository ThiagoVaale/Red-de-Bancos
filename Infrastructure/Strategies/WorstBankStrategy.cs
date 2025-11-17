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
    public sealed class WorstBankStrategy : IExternalTransferStrategy
    {
        private readonly ILogger<WorstBankStrategy> _logger;

        public WorstBankStrategy(ILogger<WorstBankStrategy> logger)
        {
            _logger = logger;
        }

        public BankCode SupportedBank => BankCode.WorstBank;

        public async Task<ExternalTransferResultDto> SendAsync(
            ExternalTransferRequestDto transfer,
            CancellationToken ct)
        {
            _logger.LogInformation("WorstBank: Intentando transferencia simulada...");
                
            try
            {
                await Task.Delay(2000, ct);

                if (new Random().NextDouble() < 0.1)
                {
                    var externalRef = $"WORST-{Guid.NewGuid():N}";
                    _logger.LogInformation("WorstBank: ¡Milagro! Transferencia exitosa.");
                    return ExternalTransferResultDto.Success(externalRef);
                }

                _logger.LogError("WorstBank: Fallo catastrófico simulado.");
                return ExternalTransferResultDto.Failure("WorstBank failed to process transfer.");
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning("WorstBank: Transferencia cancelada.");
                return ExternalTransferResultDto.Failure("Transfer cancelled by caller.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WorstBank: Error inesperado.");
                return ExternalTransferResultDto.Failure("Unexpected error contacting WorstBank.");
            }
        }
    }
}
