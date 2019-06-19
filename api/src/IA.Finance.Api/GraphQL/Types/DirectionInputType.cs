using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class DirectionInputType : InputObjectGraphType<DirectionDto>
    {
        public DirectionInputType()
        {
            Name = "DirectionInput";

            Field(x => x.Id, false, typeof(NonNullGraphType<IdGraphType>));
            Field(x => x.Name, true);
        }
    }
}