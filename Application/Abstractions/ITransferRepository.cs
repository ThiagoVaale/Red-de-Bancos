using Domine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ITransferRepository
    {
        Task AddAsync(Transfer transfer, CancellationToken ct);
        Task<Transfer?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(Transfer transfer, CancellationToken ct);
        Task<bool> ExistsByExternalRefAsync(string externalRef, CancellationToken ct);
    }
}
