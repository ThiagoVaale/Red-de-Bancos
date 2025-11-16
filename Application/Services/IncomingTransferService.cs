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
    public sealed class IncomingTransferService
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransferRepository _transfers;
        private readonly IUnitOfWork _uow;

        public IncomingTransferService(
            IAccountRepository accounts,
            ITransferRepository transfers,
            IUnitOfWork uow)
        {
            _accounts = accounts;
            _transfers = transfers;
            _uow = uow;
        }

        public async Task<Guid> RegisterAsync(IncomingExternalTransferDto dto, CancellationToken ct)
        {
            if (await _transfers.ExistsByExternalRefAsync(dto.ExternalReference, ct))
                throw new ValidationException("Duplicate external reference.");

            var to = await _accounts.GetByCbuAsync(dto.ToAccountCbu, ct)
                     ?? throw new NotFoundException("Destination account not found.");
            if (!to.IsActive)
            {
                throw new ConflictException("Inactive account.");
            }

            var money = new Money(dto.Amount, dto.Currency);

            to.Credit(money);

            var transfer = Transfer.IncomingExternal(dto.ExternalReference, money);
            transfer.MarkCompleted();

            await _accounts.UpdateAsync(to, ct);
            await _transfers.AddAsync(transfer, ct);
            await _uow.SaveChangesAsync(ct);

            return transfer.Id;
        }

        public async Task<Guid> ProcessIncomingTransfer(IncomingExternalTransferDto dto, CancellationToken ct)
        {
            if (await _transfers.ExistsByExternalRefAsync(dto.ExternalReference, ct))
                throw new ValidationException("Duplicate external reference.");

            var account = await _accounts.GetByCbuAsync(dto.ToAccountCbu, ct)
                ?? throw new NotFoundException("Destination account not found.");

            if (!account.IsActive)
                throw new ConflictException("Inactive account.");


            var money = new Money(dto.Amount, dto.Currency);

            account.Credit(money);

            var transfer = Transfer.IncomingExternal(dto.ExternalReference, money);
            transfer.MarkCompleted();

            await _accounts.UpdateAsync(account, ct);
            await _transfers.AddAsync(transfer, ct);
            await _uow.SaveChangesAsync(ct);

            return transfer.Id;
        }
    }
}
