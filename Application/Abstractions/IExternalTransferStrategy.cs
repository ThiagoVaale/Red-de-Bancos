using Application.DTOs;
using Domine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IExternalTransferStrategy
    {
        BankCode SupportedBank { get; }
        public Task<ExternalTransferResultDto> SendAsync(ExternalTransferRequestDto request, CancellationToken ct);
    }
}
