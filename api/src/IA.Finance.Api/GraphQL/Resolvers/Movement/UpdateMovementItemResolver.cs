using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class UpdateMovementItemResolver : AResolver<MovementItemDto>
    {
        private const string ArgumentName = "movementItem";

        private readonly IMovementRepository _movementRepository;

        public UpdateMovementItemResolver(IUserRepository userRepository, IMovementRepository movementRepository) :
            base(userRepository) => _movementRepository =
            movementRepository ?? throw new ArgumentNullException(nameof(movementRepository));

        public override async Task<MovementItemDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }
            
            var dto = context.GetArgument<MovementItemDto>(ArgumentName);

            var movement = await _movementRepository.FindById(dto.MovementId).ConfigureAwait(false);
            var movementItem = await _movementRepository.FindMovementItemById(dto.Id).ConfigureAwait(false);
            var user = await GetUser(context).ConfigureAwait(false);
                
            if (movement == null || movementItem == null || movement.OwnerId != user.Id)
            {
                throw new ResolverException("Movement item not found.");
            }
                
            movementItem.SetNewMovementId(dto.MovementId);
            movementItem.SetNewDate(dto.Date);
            movementItem.SetNewAmount(dto.Amount);
            movementItem.SetNewDescription(dto.Description);
                
            _movementRepository.Update(movementItem);
            await _movementRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return dto.Populate(movementItem);
        }
    }
}