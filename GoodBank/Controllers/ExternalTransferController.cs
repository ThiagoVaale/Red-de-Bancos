using Application.Abstractions;
using Application.DTOs;
using Application.Services;
using GoodBank.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodBank.Controllers
{
    [Route("api/transfers/external")]
    [ApiController]
    public sealed class ExternalTransferController : ControllerBase
    {
        private readonly ExternalTransferService _externalService;
        private readonly InternalTransferService _internalService;
        private readonly ITransferRepository _transfers;

        public ExternalTransferController(ExternalTransferService externalService, InternalTransferService internalService, ITransferRepository transfer)
        {
            _externalService = externalService;
            _internalService = internalService;
            _transfers = transfer;
        }

        [HttpPost]
        public async Task<IActionResult> External(
            [FromBody] ExternalTransferRequestDto request,
            CancellationToken ct)
        {
            var id = await _externalService.CreateAsync(request, ct);

            var transfer = await _transfers.GetByIdAsync(id, ct);

            if (transfer is null)
                return NotFound(new { error = "Transfer not found" });

            var response = new TransferResponseDto(
                transfer.Id,
                transfer.Status,
                transfer.Direction,
                transfer.Amount.Amount,
                transfer.Amount.Currency,
                transfer.CreatedAt,
                transfer.ExternalReference
            );

            return Ok(response);
        }
    }
}
