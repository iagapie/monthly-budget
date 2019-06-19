using System.Threading.Tasks;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.Jwt
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(User user);
    }
}