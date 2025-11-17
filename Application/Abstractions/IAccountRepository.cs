using Domine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Account?> GetByCbuAsync(string cbu, CancellationToken ct);
        Task UpdateAsync(Account account, CancellationToken ct);
    }
}
