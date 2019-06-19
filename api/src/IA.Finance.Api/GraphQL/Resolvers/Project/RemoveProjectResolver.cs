using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Project
{
    public class RemoveProjectResolver : AResolver<ProjectDto>
    {
        private const string ArgumentName = "id";
        
        private readonly IProjectRepository _projectRepository;

        public RemoveProjectResolver(IUserRepository userRepository, IProjectRepository projectRepository) :
            base(userRepository) => _projectRepository =
            projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        public override async Task<ProjectDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }

            var id = context.GetArgument<long>(ArgumentName);
            var project = await _projectRepository.FindById(id).ConfigureAwait(false);
            var user = await GetUser(context).ConfigureAwait(false);

            if (project == null || project.OwnerId != user.Id)
            {
                throw new ResolverException("Project not found.");
            }
                
            _projectRepository.Remove(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                
            return new ProjectDto().Populate(project);
        }
    }
}