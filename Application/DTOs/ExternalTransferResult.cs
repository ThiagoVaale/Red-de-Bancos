namespace GoodBank.Application.Models;

/// <summary>
/// Representa el resultado de una operación de transferencia externa.
/// </summary>
public record ExternalTransferResult
{
    public bool IsSuccess { get; init; }
    public string? ExternalReferenceId { get; init; }
    public string? ErrorMessage { get; init; }

    public bool IsTransientError { get; init; }
    // --- Factory Methods para legibilidad ---

    /// <summary>
    /// Crea un resultado de éxito.
    /// </summary>
    public static ExternalTransferResult Success(string externalReferenceId) => new()
    {
        IsSuccess = true,
        ExternalReferenceId = externalReferenceId
    };

    /// <summary>
    /// Crea un resultado de fracaso.
    /// </summary>
    public static ExternalTransferResult Failure(string errorMessage, bool isTransient = false) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        IsTransientError = isTransient // 🚨 Ahora acepta el parámetro
    };
}