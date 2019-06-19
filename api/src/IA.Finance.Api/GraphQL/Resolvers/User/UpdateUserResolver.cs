using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.User
{
    public class UpdateUserResolver : AResolver<UserDto>
    {
        private const string ArgumentName = "user";
        
        private readonly IUserRepository _userRepository;
        
        public UpdateUserResolver(IUserRepository userRepository) : base(userRepository) => _userRepository = userRepository;

        public override async Task<UserDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }
            
            var dto = context.GetArgument<UserDto>(ArgumentName);

            var user = await _userRepository.FindById(dto.Id).ConfigureAwait(false);

            if (user == null)
            {
                throw new ResolverException("User not found.");
            }

            var loggedUser = await GetUser(context).ConfigureAwait(false);
            
            if (!(loggedUser.Role == "admin" || loggedUser.Id == user.Id))
            {
                throw new ResolverException("User not found.");
            }
            
            user.SetNewEmail(dto.Email);
            user.SetNewUserName(dto.UserName);
            user.SetNewFirstName(dto.FirstName);
            user.SetNewLastName(dto.LastName);
            
            if (loggedUser.Role == "admin" && !string.IsNullOrWhiteSpace(dto.Role))
            {
                user.SetNewRole(dto.Role);
            }

            await _userRepository.Update(user).ConfigureAwait(false);

            return dto.Populate(user);
        }
    }
}