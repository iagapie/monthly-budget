using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class ProjectType : ObjectGraphType<ProjectDto>
    {
        public ProjectType()
        {
            Name = "Project";
            
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OwnerId);
            Field(x => x.Name);
            Field(x => x.Currency);
            Field(x => x.CreatedAt);
            Field(x => x.UpdatedAt, true);
        }
    }
}