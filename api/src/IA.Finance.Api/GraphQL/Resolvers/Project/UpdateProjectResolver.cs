using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Project
{
    public class UpdateProjectResolver : AResolver<ProjectDto>
    {
        private const string ArgumentName = "project";
        
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectResolver(IUserRepository userRepository, IProjectRepository projectRepository) :
            base(userRepository) => _projectRepository =
            projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        public override async Task<ProjectDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }
            
            var dto = context.GetArgument<ProjectDto>(ArgumentName);
            
            var project = await _projectRepository.FindById(dto.Id).ConfigureAwait(false);
            
            var user = await GetUser(context).ConfigureAwait(false);

            if (project == null || project.OwnerId != user.Id)
            {
                throw new ResolverException("Project not found.");
            }

            project.SetNewName(dto.Name);
            project.SetNewCurrency(dto.Currency);
                
            _projectRepository.Update(project);
            
            await _projectRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return dto.Populate(project);
        }
    }
}