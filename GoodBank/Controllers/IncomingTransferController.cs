using Application.DTOs;
using Application.Services;
using GoodBank.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodBank.Controllers
{
    [Route("api/transfers/incoming")]
    [ApiController]
    public sealed class IncomingTransferController : ControllerBase
    {
        private readonly IncomingTransferService _service;

        public IncomingTransferController(IncomingTransferService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] IncomingTransferRequestApiDto dto, CancellationToken ct)
        {
            var appDto = new IncomingExternalTransferDto(
                dto.ExternalBankCode,
                dto.ExternalReference,
                dto.ToAccountCbu,
                dto.Amount,
                dto.Currency
            );

            var id = await _service.RegisterAsync(appDto, ct);

            return Ok(new { TransferId = id });
        }
    }
}
