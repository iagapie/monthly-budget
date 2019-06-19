using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class CreateMovementResolver : AResolver<MovementDto>
    {
        private const string ArgumentName = "movement";

        private readonly IMovementRepository _movementRepository;

        public CreateMovementResolver(IUserRepository userRepository, IMovementRepository movementRepository) :
            base(userRepository) => _movementRepository =
            movementRepository ?? throw new ArgumentNullException(nameof(movementRepository));

        public override async Task<MovementDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }
            
            var dto = context.GetArgument<MovementDto>(ArgumentName);

            var user = await GetUser(context).ConfigureAwait(false);

            var movement = new Domain.AggregatesModel.MovementAggregate.Movement(dto.ProjectId, dto.Name,
                dto.Direction.Id, dto.PlanAmount, user.Id);
                
            _movementRepository.Add(movement);
                
            await _movementRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                
            dto.Direction = new DirectionDto().Populate(Enumeration.FromValue<Direction>(movement.DirectionId));

            return dto.Populate(movement, () => movement.Direction, () => movement.MovementItems);
        }
    }
}