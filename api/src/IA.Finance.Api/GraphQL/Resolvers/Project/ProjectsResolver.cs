using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Project
{
    public class ProjectsResolver : AResolver<IEnumerable<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsResolver(IUserRepository userRepository, IProjectRepository projectRepository) :
            base(userRepository) => _projectRepository =
            projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        public override async Task<IEnumerable<ProjectDto>> Resolve(ResolveFieldContext<object> context)
        {
            var user = await GetUser(context).ConfigureAwait(false);

            var projects = await _projectRepository.FindByOwnerId(user.Id).ConfigureAwait(false);

            return projects.Select(x => new ProjectDto().Populate(x));
        }
    }
}