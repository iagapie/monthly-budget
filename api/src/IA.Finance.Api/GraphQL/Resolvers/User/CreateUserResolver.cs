using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.User
{
    public class CreateUserResolver : AResolver<UserDto>
    {
        private const string ArgumentUser = "user";
        private const string ArgumentPassword = "password";
        
        private readonly IUserRepository _userRepository;
        
        public CreateUserResolver(IUserRepository userRepository) : base(userRepository) => _userRepository = userRepository;

        public override async Task<UserDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentUser))
            {
                throw new ResolverException($"Argument '{ArgumentUser}' not found.");
            }
            
            if (!context.HasArgument(ArgumentPassword))
            {
                throw new ResolverException($"Argument '{ArgumentPassword}' not found.");
            }
            
            var dto = context.GetArgument<UserDto>(ArgumentUser);
            var password = context.GetArgument<string>(ArgumentPassword);

            if (string.IsNullOrWhiteSpace(dto.Role))
            {
                dto.Role = "user";
            }

            await _userRepository
                .Add(dto.UserName, dto.Email, password, dto.Role, dto.FirstName, dto.LastName)
                .ConfigureAwait(false);

            var user = await _userRepository.FindByUserName(dto.UserName).ConfigureAwait(false);
            
            if (user == null)
            {
                throw new ResolverException("User not found.");
            }

            return dto.Populate(user);
        }
    }
}