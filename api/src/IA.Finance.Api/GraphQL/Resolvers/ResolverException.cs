using System;

namespace IA.Finance.Api.GraphQL.Resolvers
{
    public class ResolverException : Exception
    {
        public ResolverException(string message) : base(message)
        {
        }
    }
}