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
        private readonly ExternalTransferService _service;

        public ExternalTransferController(ExternalTransferService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExternalTransferRequestApiDto dto, CancellationToken ct)
        {
            var appDto = new ExternalTransferRequestDto(
                dto.FromAccountId,
                dto.ExternalBankCode,
                dto.ExternalAccountRef,
                dto.Amount,
                dto.Currency
            );

            var id = await _service.CreateAsync(appDto, ct);

            return Ok(new { TransferId = id });
        }
    }
}
