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
    public sealed class BadBankStrategy : IExternalTransferStrategy 
    {
        private readonly ILogger<BadBankStrategy> _logger;

        public BadBankStrategy(ILogger<BadBankStrategy> logger)
        {
            _logger = logger;
        }

        public BankCode SupportedBank => BankCode.BadBank;

        public async Task<ExternalTransferResultDto> SendAsync(ExternalTransferRequestDto transfer, CancellationToken ct)
        {
            _logger.LogInformation(
                $"BadBank: Iniciando transferencia simulada por {transfer.Amount}"); 
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(200), ct);

                var externalRef = $"BADBANK-{Guid.NewGuid():N}";
                _logger.LogInformation(
                    "BadBank simulated success. ExternalRef: {ExternalRef}", externalRef);

                return ExternalTransferResultDto.Success(externalRef);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning("BadBank transfer cancelled by caller.");
                return ExternalTransferResultDto.Failure("Transfer cancelled by caller.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when contacting BadBank.");
                return ExternalTransferResultDto.Failure("Unexpected error contacting BadBank.");
            }
        }
    }
}
