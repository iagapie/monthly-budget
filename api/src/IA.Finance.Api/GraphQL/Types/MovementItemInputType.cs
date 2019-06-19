using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class MovementItemInputType : InputObjectGraphType<MovementItemDto>
    {
        public MovementItemInputType()
        {
            Name = "MovementItemInput";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.MovementId);
            Field(x => x.Date);
            Field(x => x.Amount);
            Field(x => x.Description, true);
        }
    }
}