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
    public sealed class Account
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Cbu { get; private set; }
        public decimal Balance { get; private set; }
        public Currency Currency { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsActive { get; private set; } = true;

        private Account() { }

        public Account(string cbu, decimal initialBalance, Currency currency, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(cbu))
                throw new DomainException("CBU is required.");

            if (initialBalance < 0)
                throw new DomainException("Initial balance cannot be negative.");

            Cbu = cbu.Trim();
            Balance = decimal.Round(initialBalance, 2);
            Currency = currency;
            UserId = userId;
        }

        public void Credit(Money amount)
        {
            if (amount.Currency != Currency)
                throw new DomainException("Currency mismatch.");
            Balance = decimal.Round(Balance + amount.Amount, 2);
        }

        public void Debit(Money amount)
        {
            if (amount.Currency != Currency)
                throw new DomainException("Currency mismatch.");
            if (Balance < amount.Amount)
                throw new DomainException("Insufficient funds.");
            Balance = decimal.Round(Balance - amount.Amount, 2);
        }

        public void Deactivate() => IsActive = false;
    }
}
