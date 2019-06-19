using System;
using IA.Finance.Domain.Exceptions;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.MovementAggregate
{
    public class MovementItem : Entity
    {
        private long _movementId;
        
        private DateTimeOffset _date;

        private decimal _amount;

        private string _description;
        
        private long? _ownerId;

        public long MovementId => _movementId;

        public DateTimeOffset Date => _date;

        public decimal Amount => _amount;

        public string Description => _description;
        
        public long? OwnerId => _ownerId;

        public MovementItem(long movementId, decimal amount, DateTimeOffset date, long? ownerId = null, string description = null)
        {
            SetNewMovementId(movementId);
            SetNewAmount(amount);
            SetNewDate(date);
            SetNewDescription(description);
            SetNewOwnerId(ownerId);
        }

        public void SetNewMovementId(long movementId)
        {
            if (movementId <= 0)
            {
                throw new FinanceDomainException("Invalid movement id.");
            }

            _movementId = movementId;
        }

        public void SetNewAmount(decimal amount)
        {
            if (amount <= decimal.Zero)
            {
                throw new FinanceDomainException("Invalid amount.");
            }

            _amount = amount;
        }

        public void SetNewDate(DateTimeOffset date) => _date = date;

        public void SetNewDescription(string description) => _description = description;
        
        public void SetNewOwnerId(long? ownerId) => _ownerId = ownerId;
    }
}