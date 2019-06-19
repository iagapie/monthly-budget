using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers
{
    public abstract class AResolver<T> : IResolver<T>
    {
        private readonly IUserRepository _userRepository;

        protected AResolver(IUserRepository userRepository) => 
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        protected async Task<Domain.AggregatesModel.UserAggregate.User> GetUser(ResolveFieldContext<object> context)
        {
            var user = await _userRepository.FindByIdentityId(context.IdentityId()).ConfigureAwait(false);

            if (user == null)
            {
                throw new ResolverException("User not found.");
            }

            return user;
        }

        public abstract Task<T> Resolve(ResolveFieldContext<object> context);
    }
}