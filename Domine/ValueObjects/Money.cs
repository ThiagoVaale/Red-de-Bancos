    using Domine.Enums;
    using Domine.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Domine.ValueObjects
    {
        public sealed record Money
        {
            public decimal Amount { get; init; }
            public Currency Currency { get; init; }

            public Money(decimal amount, Currency currency)
            {
                if (amount <= 0)
                    throw new DomainException("Amount must be greater than zero.");

                if (decimal.Round(amount, 2) != amount)
                    throw new DomainException("Amount must have at most 2 decimal places.");

                Amount = amount;
                Currency = currency;
            }

            public Money Negate() => new(-Amount, Currency);

            public override string ToString() => $"{Amount:F2} {Currency}";
        }
    }
