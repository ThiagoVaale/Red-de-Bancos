using Application.Abstractions;
using Application.DTOs;
using GoodBank.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GoodBank.Controllers
{
    [ApiController]
    [Route("api/public/transfers")]
    public class PublicTransfersController : ControllerBase
    {
        private readonly IIncomingTransferService _service;

        public PublicTransfersController(IIncomingTransferService service)
        {
            _service = service;
        }

        [HttpPost("incoming")]
        public async Task<IActionResult> Receive([FromBody] IncomingTransferRequestApiDto dto,CancellationToken ct)
        {
            var appDto = new IncomingExternalTransferDto(
                dto.ExternalBankCode,
                dto.ExternalReference,
                dto.ToAccountCbu,
                dto.Amount,
                dto.Currency
            );

            var id = await _service.ProcessIncomingTransfer(appDto, ct);

            return Ok(new { TransferId = id });
        }
    }
}
