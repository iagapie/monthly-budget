using System.Threading.Tasks;
using GraphQL.Types;

namespace IA.Finance.Api.GraphQL.Resolvers
{
    public interface IResolver<T>
    {
        Task<T> Resolve(ResolveFieldContext<object> context);
    }
}