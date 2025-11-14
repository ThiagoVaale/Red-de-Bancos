using Application.DTOs;
using Application.Services;
using GoodBank.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodBank.Controllers
{
    [Route("api/transfers/internal")]
    [ApiController]
    public sealed class InternalTransferController : ControllerBase
    {
        private readonly InternalTransferService _service;

        public InternalTransferController(InternalTransferService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InternalTransferRequestApiDto dto, CancellationToken ct)
        {
            var appDto = new InternalTransferRequestDto(
                dto.FromAccountId,
                dto.ToAccountId,
                dto.Amount,
                dto.Currency
            );

            var id = await _service.CreateAsync(appDto, ct);

            return Ok(new { TransferId = id });
        }
    }
}
