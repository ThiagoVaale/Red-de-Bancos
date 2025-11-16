// 1. Agrega los 'usings' correctos
using Application.DTOs;
using Domine.Entities;
using Domine.Enums;
using GoodBank.Application.Interfaces; // Para la Interfaz
using GoodBank.Application.Models;      // Para el ExternalTransferResult
using Microsoft.Extensions.Logging;
using System.Net.Http;

// Asumo que está en Infrastructure/Strategies
namespace GoodBank.Infrastructure.Strategies
{
    // 2. Asegúrate de implementar la interfaz
    public class BadBankStrategy : IExternalTransferStrategy
    {
        private readonly ILogger<BadBankStrategy> _logger;

        public BadBankStrategy(ILogger<BadBankStrategy> logger)
        {
            _logger = logger;
        }

        // 3. Implementa la propiedad EXACTAMENTE como la interfaz lo pide
        public BankCode SupportedBank => BankCode.BadBank;

        // 4. Implementa el método EXACTAMENTE como la interfaz lo pide
        public async Task<ExternalTransferResult> SendAsync(ExternalTransferRequestDto transfer, CancellationToken ct)
{
            _logger.LogInformation(
                "BadBank: Iniciando transferencia simulada para {TransferId} por {Amount}",
                transfer.transferId,
                transfer.Amount); // Ahora tienes la entidad completa
            try
            {
                // Simulamos una pequeña latencia de red
                await Task.Delay(TimeSpan.FromMilliseconds(200), ct);

                // El POST real usaría datos de la entidad 'transfer'
                // var requestDto = new { 
                //    Amount = transfer.Amount.Value, 
                //    Currency = transfer.Amount.Currency,
                //    Destination = transfer.DestinationAccount 
                // };
                // using var response = await _client.PostAsJsonAsync("/api/transfers", requestDto, ct);

                var externalRef = $"BADBANK-{Guid.NewGuid():N}";
                _logger.LogInformation(
                    "BadBank simulated success. ExternalRef: {ExternalRef}", externalRef);

                // 5. Devuelve el OBJETO 'ExternalTransferResult', no la tupla
                return ExternalTransferResult.Success(externalRef);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning("BadBank transfer cancelled by caller.");
                return ExternalTransferResult.Failure("Transfer cancelled by caller.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when contacting BadBank.");
                return ExternalTransferResult.Failure("Unexpected error contacting BadBank.");
            }
        }
    }
}