using Application.Abstractions;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly GoodBankDbContext _db;

        public UnitOfWork(GoodBankDbContext db)
        {
            _db = db;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
