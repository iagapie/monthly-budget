using System.Security.Claims;
using GraphQL.Authorization;

namespace IA.Finance.Api.GraphQL.Authorization
{
    public class UserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}