using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.User
{
    public class UserResolver : AResolver<UserDto>
    {
        private const string ArgumentName = "id";
        
        private readonly IUserRepository _userRepository;
        
        public UserResolver(IUserRepository userRepository) : base(userRepository) => _userRepository = userRepository;

        public override async Task<UserDto> Resolve(ResolveFieldContext<object> context)
        {
            var loggedUser = await GetUser(context).ConfigureAwait(false);

            if (loggedUser.Role == "admin" && context.HasArgument(ArgumentName))
            {
                var id = context.GetArgument<long?>(ArgumentName);

                if (id.HasValue)
                {
                    var user = await _userRepository.FindById(id.Value).ConfigureAwait(false);

                    if (user == null)
                    {
                        throw new ResolverException("User not found.");
                    }
                    
                    return new UserDto().Populate(user);
                }
            }
            
            return new UserDto().Populate(loggedUser);
        }
    }
}