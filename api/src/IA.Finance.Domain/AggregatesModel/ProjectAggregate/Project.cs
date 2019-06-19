using System;
using IA.Finance.Domain.Exceptions;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.ProjectAggregate
{
    public class Project : Entity, IAggregateRoot
    {
        private string _name;

        private string _currency;

        private long _ownerId;

        public string Name => _name;

        public string Currency => _currency;

        public long OwnerId => _ownerId;

        public Project(long ownerId, string name, string currency)
        {
            if (ownerId <= 0)
            {
                throw new FinanceDomainException("Invalid owner ID.");
            }

            _ownerId = ownerId;
            
            SetNewName(name);
            SetNewCurrency(currency);
        }

        public void SetNewName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new FinanceDomainException("Invalid name.");
            }

            _name = name;
        }
        
        public void SetNewCurrency(string currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new FinanceDomainException("Invalid currency.");
            }

            _currency = currency.ToUpperInvariant();
        }
    }
}