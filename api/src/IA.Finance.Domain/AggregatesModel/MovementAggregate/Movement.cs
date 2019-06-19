using System;
using System.Collections.Generic;
using IA.Finance.Domain.Exceptions;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.MovementAggregate
{
    public class Movement : Entity, IAggregateRoot
    {
        private long _projectId;

        private int _directionId;

        private string _name;

        private decimal _planAmount;
        
        private long? _ownerId;

        private readonly List<MovementItem> _movementItems;

        public long ProjectId => _projectId;

        public int DirectionId => _directionId;

        public string Name => _name;

        public decimal PlanAmount => _planAmount;

        public Direction Direction { get; private set; }
        
        public long? OwnerId => _ownerId;

        public IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected Movement() => _movementItems = new List<MovementItem>();

        public Movement(long projectId, string name, int directionId, decimal planAmount = decimal.Zero, long? ownerId = null)
        {
            SetNewName(name);
            SetNewPlanAmount(planAmount);
            SetNewOwnerId(ownerId);
            
            _projectId = projectId;
            _directionId = directionId;
        }

        public MovementItem AddMovementItem(decimal amount, DateTimeOffset date, long? ownerId = null, string description = null)
        {
            var movementItem = new MovementItem(Id, amount, date, ownerId, description);
            _movementItems.Add(movementItem);
            return movementItem;
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

        public void SetNewPlanAmount(decimal planAmount)
        {
            if (planAmount < decimal.Zero)
            {
                throw new FinanceDomainException("Invalid plan amount.");
            }

            _planAmount = planAmount;
        }
        
        public void SetNewOwnerId(long? ownerId) => _ownerId = ownerId;
    }
}