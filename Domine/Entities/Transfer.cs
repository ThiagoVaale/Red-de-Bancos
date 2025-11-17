using Domine.Enums;
using Domine.Exceptions;
using Domine.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domine.Entities
{
    public sealed class Transfer
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid? FromAccountId { get; private set; }
        public Guid? ToAccountId { get; private set; }
        public Money Amount { get; private set; }
        public TransferStatus Status { get; private set; } = TransferStatus.Pending;
        public TransferDirection Direction { get; private set; }
        public BankCode? ExternalBankCode { get; private set; }
        public string? ExternalReference { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; private set; }
        public DateTime? CanceledAt { get; private set; }
        public string? Reason { get; private set; }

        private Transfer() { }

        public static Transfer Internal(Guid fromAccountId, Guid toAccountId, Money amount)
        {
            if (fromAccountId == toAccountId)
                throw new DomainException("Cannot transfer to the same account.");

            return new Transfer
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Direction = TransferDirection.Internal
            };
        }

        public static Transfer OutgoingExternal(Guid fromAccountId, BankCode bankCode, Money amount, string? externalRef = null)
            => new()
            {
                FromAccountId = fromAccountId,
                Amount = amount,
                Direction = TransferDirection.OutgoingExternal,
                ExternalBankCode = bankCode,
                ExternalReference = externalRef
            };

        public static Transfer IncomingExternal(string externalRef, Money amount)
            => new()
            {
                Amount = amount,
                Direction = TransferDirection.IncomingExternal,
                ExternalReference = externalRef
            };

        public void MarkCompleted()
        {
            Status = TransferStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail(string reason)
        {
            Status = TransferStatus.Failed;
            Reason = reason;
        }

        public void Cancel(string reason)
        {
            Status = TransferStatus.Cancelled;
            Reason = reason;
            CanceledAt = DateTime.UtcNow;
        }

        public void SetExternalReference(string reference)
        {
            ExternalReference = reference;
        }
    }
}
