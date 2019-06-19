using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class RemoveMovementResolver : AResolver<MovementDto>
    {
        private const string ArgumentName = "id";

        private readonly IMovementRepository _movementRepository;

        public RemoveMovementResolver(IUserRepository userRepository, IMovementRepository movementRepository) :
            base(userRepository) => _movementRepository =
            movementRepository ?? throw new ArgumentNullException(nameof(movementRepository));

        public override async Task<MovementDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }

            var id = context.GetArgument<long>(ArgumentName);
            var movement = await _movementRepository.FindById(id).ConfigureAwait(false);
            var user = await GetUser(context).ConfigureAwait(false);
                
            if (movement == null || movement.OwnerId != user.Id)
            {
                throw new ResolverException("Movement not found.");
            }
                
            _movementRepository.Remove(movement);
            await _movementRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                
            var dto = new MovementDto
            {
                Direction = new DirectionDto().Populate(movement.Direction),
                MovementItems = movement.MovementItems.Select(x => new MovementItemDto().Populate(x)).ToList()
            };

            return dto.Populate(movement, () => movement.Direction, () => movement.MovementItems);
        }
    }
}