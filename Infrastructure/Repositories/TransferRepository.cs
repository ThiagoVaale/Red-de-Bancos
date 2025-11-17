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
    public sealed class TransferRepository : ITransferRepository
    {
        private readonly GoodBankDbContext _db;

        public TransferRepository(GoodBankDbContext db)
        {
            _db = db;
        }

        public Task<Transfer?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Transfers.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(Transfer transfer, CancellationToken ct)
            => await _db.Transfers.AddAsync(transfer, ct);

        public async Task UpdateAsync(Transfer transfer, CancellationToken ct)
        {
            _db.Transfers.Update(transfer);
            await Task.CompletedTask;
        }

        public Task<bool> ExistsByExternalRefAsync(string externalRef, CancellationToken ct)
            => _db.Transfers.AnyAsync(x => x.ExternalReference == externalRef, ct);
    }
}
