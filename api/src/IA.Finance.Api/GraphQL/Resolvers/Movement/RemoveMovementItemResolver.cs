using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class RemoveMovementItemResolver : AResolver<MovementItemDto>
    {
        private const string ArgumentName = "id";

        private readonly IMovementRepository _movementRepository;

        public RemoveMovementItemResolver(IUserRepository userRepository, IMovementRepository movementRepository) :
            base(userRepository) => _movementRepository =
            movementRepository ?? throw new ArgumentNullException(nameof(movementRepository));

        public override async Task<MovementItemDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }

            var id = context.GetArgument<long>(ArgumentName);
            var movementItem = await _movementRepository.FindMovementItemById(id).ConfigureAwait(false);
            var user = await GetUser(context).ConfigureAwait(false);
                
            if (movementItem == null || movementItem.OwnerId != user.Id)
            {
                throw new ResolverException("Movement item not found.");
            }
                
            _movementRepository.Remove(movementItem);
            await _movementRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return new MovementItemDto().Populate(movementItem);
        }
    }
}