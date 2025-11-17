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
    public sealed class ExternalTransferService
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransferRepository _transfers;
        private readonly IExternalTransferGateway _gateway;
        private readonly IUnitOfWork _uow;

        public ExternalTransferService(
            IAccountRepository accounts,
            ITransferRepository transfers,
            IExternalTransferGateway gateway,
            IUnitOfWork uow)
        {
            _accounts = accounts;
            _transfers = transfers;
            _gateway = gateway;
            _uow = uow;
        }

        public async Task<Guid> CreateAsync(ExternalTransferRequestDto dto, CancellationToken ct)
        {
            if (dto.Amount <= 0)
            {
                throw new ValidationException("Amount must be greater than zero.");
            }

            var from = await _accounts.GetByIdAsync(dto.FromAccountId, ct)
                       ?? throw new NotFoundException("From account not found.");

            if (!from.IsActive)
            {
                throw new ConflictException("Inactive account.");
            }

            var money = new Money(dto.Amount, dto.Currency);

            var transfer = Transfer.OutgoingExternal(from.Id, dto.ExternalBankCode, money, dto.ExternalAccountRef);
            await _transfers.AddAsync(transfer, ct);
            await _uow.SaveChangesAsync(ct);


            var result = await _gateway.SendAsync(dto, ct);

            transfer = (await _transfers.GetByIdAsync(transfer.Id, ct))!;

            if (result.success)
            {
                from.Debit(money);
                await _accounts.UpdateAsync(from, ct);

                transfer.MarkCompleted();
                transfer.SetExternalReference(result.externalReference);
            }
            else
            {
                transfer.Fail(result.error ?? "External transfer failed.");
            }

            await _transfers.UpdateAsync(transfer, ct);
            await _uow.SaveChangesAsync(ct);

            return transfer.Id;
        }
    }
}
