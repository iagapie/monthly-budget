using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class ProjectInputType : InputObjectGraphType<ProjectDto>
    {
        public ProjectInputType()
        {
            Name = "ProjectInput";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.Currency);
        }
    }
}