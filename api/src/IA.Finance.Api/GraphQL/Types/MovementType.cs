using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class MovementType : ObjectGraphType<MovementDto>
    {
        public MovementType()
        {
            Name = "Movement";
            
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OwnerId, true);
            Field(x => x.ProjectId);
            Field(x => x.Name);
            Field(x => x.CreatedAt);
            Field(x => x.UpdatedAt, true);
            Field(x => x.PlanAmount);
            Field(x => x.Direction, type: typeof(NonNullGraphType<DirectionType>));
            Field(x => x.MovementItems, type: typeof(NonNullGraphType<ListGraphType<MovementItemType>>));
        }
    }
}