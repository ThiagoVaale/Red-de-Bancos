using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record ExternalTransferResultDto
    {
        public bool IsSuccess { get; init; }
        public string? ExternalReferenceId { get; init; }
        public string? ErrorMessage { get; init; }

        public bool IsTransientError { get; init; }

        public static ExternalTransferResultDto Success(string externalReferenceId) => new()
        {
            IsSuccess = true,
            ExternalReferenceId = externalReferenceId
        };

        public static ExternalTransferResultDto Failure(string errorMessage, bool isTransient = false) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            IsTransientError = isTransient
        };
    }
}
