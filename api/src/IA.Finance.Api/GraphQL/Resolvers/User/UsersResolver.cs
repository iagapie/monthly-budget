using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.User
{
    public class UsersResolver : AResolver<IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        
        public UsersResolver(IUserRepository userRepository) : base(userRepository) => _userRepository = userRepository;

        public override async Task<IEnumerable<UserDto>> Resolve(ResolveFieldContext<object> context)
        {
            var users = await _userRepository.Find().ConfigureAwait(false);
            return users.Where(x => x.IdentityId != context.IdentityId()).Select(x => new UserDto().Populate(x));
        }
    }
}