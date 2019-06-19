using System;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Project
{
    public class CreateProjectResolver : AResolver<ProjectDto>
    {
        private const string ArgumentName = "project";
        
        private readonly IProjectRepository _projectRepository;

        public CreateProjectResolver(IUserRepository userRepository, IProjectRepository projectRepository) :
            base(userRepository) => _projectRepository =
            projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        public override async Task<ProjectDto> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }
            
            var dto = context.GetArgument<ProjectDto>(ArgumentName);

            var user = await GetUser(context).ConfigureAwait(false);

            var project = new Domain.AggregatesModel.ProjectAggregate.Project(user.Id, dto.Name, dto.Currency);
                
            _projectRepository.Add(project);
                
            await _projectRepository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return dto.Populate(project);
        }
    }
}