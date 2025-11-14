using Application.Abstractions;
using Application.DTOs;
using Application.Exceptions;
using Domine.Entities;
using Domine.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public sealed class InternalTransferService
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransferRepository _transfers;
        private readonly IUnitOfWork _uow;

        public InternalTransferService(IAccountRepository accounts, ITransferRepository transfer, IUnitOfWork ouw)
        {
            _accounts = accounts;
            _transfers = transfer;
            _uow = ouw;
        }

        public async Task<Guid> CreateAsync(InternalTransferRequestDto dto, CancellationToken ct)
        {
            if(dto.Amount <= 0)
            {
                throw new ValidationException("Amount must be greater than zero.");
            }

            if(dto.FromAccountId == dto.ToAccountId)
            {
                throw new ValidationException("From and To accounts must be different.");
            }

            var from = await _accounts.GetByIdAsync(dto.FromAccountId, ct)
                   ?? throw new NotFoundException("From account not found.");

            var to = await _accounts.GetByIdAsync(dto.ToAccountId, ct)
                 ?? throw new NotFoundException("To account not found.");

            if (!from.IsActive || !to.IsActive)
            {
                throw new ConflictException("Inactive account.");
            }

            var money = new Money(dto.Amount, dto.Currency);

            from.Debit(money);
            to.Credit(money);

            var transfer = Transfer.Internal(from.Id, to.Id, money);
            transfer.MarkCompleted();

            await _accounts.UpdateAsync(from, ct);
            await _accounts.UpdateAsync(to, ct);
            await _transfers.AddAsync(transfer, ct);
            await _uow.SaveChangesAsync(ct);

            return transfer.Id;
        }
    }
}
