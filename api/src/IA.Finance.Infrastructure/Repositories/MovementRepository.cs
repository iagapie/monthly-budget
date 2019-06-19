using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace IA.Finance.Infrastructure.Repositories
{
    public class MovementRepository : IMovementRepository
    {
        private readonly FinanceContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public MovementRepository(FinanceContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        public void Add(Movement movement) => _context.Movements.Add(movement);

        public void Update(Movement movement) => _context.Entry(movement).State = EntityState.Modified;
        
        public void Remove(Movement movement) => _context.Entry(movement).State = EntityState.Deleted;
        
        public void Add(MovementItem movementItem) => _context.MovementItems.Add(movementItem);

        public void Update(MovementItem movementItem) => _context.Entry(movementItem).State = EntityState.Modified;

        public void Remove(MovementItem movementItem) => _context.Entry(movementItem).State = EntityState.Deleted;

        public async Task<Movement> FindById(long movementId)
        {
            var movement = await _context.Movements.FindAsync(movementId).ConfigureAwait(false);

            if (movement != null)
            {
                await _context.Entry(movement).Collection(e => e.MovementItems).LoadAsync().ConfigureAwait(false);
                await _context.Entry(movement).Reference(e => e.Direction).LoadAsync().ConfigureAwait(false);
            }

            return movement;
        }

        public async Task<MovementItem> FindMovementItemById(long movementItemId) =>
            await _context.MovementItems.FindAsync(movementItemId).ConfigureAwait(false);

        public async Task<IEnumerable<Movement>> FindByProjectId(long projectId) => await _context
            .Movements
            .Include(x => x.Direction)
            .Include(x => x.MovementItems)
            .Where(x => x.ProjectId == projectId)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}