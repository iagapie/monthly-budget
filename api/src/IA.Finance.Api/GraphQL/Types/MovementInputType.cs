using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class MovementInputType : InputObjectGraphType<MovementDto>
    {
        public MovementInputType()
        {
            Name = "MovementInput";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.ProjectId);
            Field(x => x.Name);
            Field(x => x.PlanAmount);
            Field(x => x.Direction, type: typeof(NonNullGraphType<DirectionInputType>));
        }
    }
}