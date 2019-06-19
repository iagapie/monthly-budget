using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class CreateMovementItemResolver : AResolver<MovementItemDto>
    {
        private const string ArgumentName = "movementItem";

        private readonly IMovementRepository _movementRepository;

        public CreateMovementItemResolver(IUserRepository userRepository, IMovementRepository movementRepository) :
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
            var user = await GetUser(context).ConfigureAwait(false);
                
            if (movement == null || movement.OwnerId != user.Id)
            {
                throw new ResolverException("Movement item not found.");
            }

            var item = movement.AddMovementItem(dto.Amount, dto.Date, user.Id, dto.Description);
                
            _movementRepository.Add(item);
            await _movementRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return dto.Populate(item);
        }
    }
}