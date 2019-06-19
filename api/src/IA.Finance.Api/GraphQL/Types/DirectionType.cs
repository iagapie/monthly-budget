using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class DirectionType : ObjectGraphType<DirectionDto>
    {
        public DirectionType()
        {
            Name = "Direction";
            
            Field(x => x.Id, false, typeof(IdGraphType));
            Field(x => x.Name);
        }
    }
}