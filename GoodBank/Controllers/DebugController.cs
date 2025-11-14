using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GoodBank.Controllers
{
    [Route("api/debug")]
    [ApiController]
    public sealed class DebugController : ControllerBase
    {
        private readonly GoodBankDbContext _db;

        public DebugController(GoodBankDbContext db)
        {
            _db = db;
        }

        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            var accounts = _db.Accounts.Select(a => new {
                a.Id,
                a.Cbu,
                a.Balance,
                a.Currency,
                a.UserId,
                a.IsActive
            }).ToList();

            return Ok(accounts);
        }
    }
}
