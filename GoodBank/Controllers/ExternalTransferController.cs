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

        public ExternalTransferController(ExternalTransferService externalService, InternalTransferService internalService)
        {
            _externalService = externalService;
            _internalService = internalService;
        }

        [HttpPost("external")]
        public async Task<IActionResult> External([FromBody] ExternalTransferRequestDto request,CancellationToken ct)
        {
            var id = await _externalService.CreateAsync(request, ct);

            return Ok(new { TransferId = id });
        }
    }
}
