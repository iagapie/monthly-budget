using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class MovementItemType : ObjectGraphType<MovementItemDto>
    {
        public MovementItemType()
        {
            Name = "MovementItem";
            
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Date);
            Field(x => x.Amount);
            Field(x => x.MovementId);
            Field(x => x.OwnerId, true);
            Field(x => x.Description, true);
            Field(x => x.CreatedAt);
            Field(x => x.UpdatedAt, true);
        }
    }
}