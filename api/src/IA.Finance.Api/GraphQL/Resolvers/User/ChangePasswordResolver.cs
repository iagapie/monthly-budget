using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.User
{
    public class ChangePasswordResolver : AResolver<bool>
    {
        private const string ArgumentId = "id";
        private const string ArgumentCurrentPassword = "currentPassword";
        private const string ArgumentNewPassword = "newPassword";
        
        private readonly IUserRepository _userRepository;

        public ChangePasswordResolver(IUserRepository userRepository) : base(userRepository) =>
            _userRepository = userRepository;

        public override async Task<bool> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentCurrentPassword))
            {
                throw new ResolverException($"Argument '{ArgumentCurrentPassword}' not found.");
            }
            
            if (!context.HasArgument(ArgumentNewPassword))
            {
                throw new ResolverException($"Argument '{ArgumentNewPassword}' not found.");
            }

            var loggedUser = await GetUser(context).ConfigureAwait(false);

            var currentPassword = context.GetArgument<string>(ArgumentCurrentPassword);
            var newPassword = context.GetArgument<string>(ArgumentNewPassword);
            var identityId = loggedUser.IdentityId;

            if (loggedUser.Role == "admin" && context.HasArgument(ArgumentId))
            {
                var id = context.GetArgument<long?>(ArgumentId);

                if (id.HasValue)
                {
                    var user = await _userRepository.FindById(id.Value).ConfigureAwait(false);

                    if (user == null)
                    {
                        throw new ResolverException("User not found.");
                    }

                    identityId = user.IdentityId;
                }
            }

            return await _userRepository
                .ChangePassword(identityId, currentPassword, newPassword)
                .ConfigureAwait(false);
        }
    }
}