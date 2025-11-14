using Application.Abstractions;
using Domine.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly GoodBankDbContext _db;

        public AccountRepository(GoodBankDbContext db)
        {
            _db = db;
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            Console.WriteLine($"[DEBUG] Buscando cuenta con ID: {id}");

            var acc = await _db.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (acc == null)
                Console.WriteLine("[DEBUG] AccountRepository devolvió NULL.");

            return acc;
        }

        public Task<Account?> GetByCbuAsync(string cbu, CancellationToken ct)
            => _db.Accounts.FirstOrDefaultAsync(x => x.Cbu == cbu, ct);

        public async Task UpdateAsync(Account account, CancellationToken ct)
        {
            _db.Accounts.Update(account);
            await Task.CompletedTask;
        }
    }
}
