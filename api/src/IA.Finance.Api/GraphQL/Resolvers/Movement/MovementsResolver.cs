using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.Models;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;

namespace IA.Finance.Api.GraphQL.Resolvers.Movement
{
    public class MovementsResolver : AResolver<IEnumerable<MovementDto>>
    {
        private const string ArgumentName = "projectId";

        private readonly IProjectRepository _projectRepository;

        private readonly IMovementRepository _movementRepository;

        public MovementsResolver(IUserRepository userRepository, IProjectRepository projectRepository,
            IMovementRepository movementRepository) : base(userRepository)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _movementRepository = movementRepository ?? throw new ArgumentNullException(nameof(movementRepository));
        }

        public override async Task<IEnumerable<MovementDto>> Resolve(ResolveFieldContext<object> context)
        {
            if (!context.HasArgument(ArgumentName))
            {
                throw new ResolverException($"Argument '{ArgumentName}' not found.");
            }

            var projectId = context.GetArgument<long>(ArgumentName);
            var project = await _projectRepository.FindById(projectId).ConfigureAwait(false);
            var user = await GetUser(context).ConfigureAwait(false);
                
            if (project == null || project.OwnerId != user.Id)
            {
                throw new ResolverException("Project not found.");
            }

            var movements = await _movementRepository.FindByProjectId(project.Id).ConfigureAwait(false);

            return movements.Select(x => new MovementDto
            {
                MovementItems = x.MovementItems.Select(item => new MovementItemDto().Populate(item)).ToList()
            }.Populate(x, () => x.MovementItems));
        }
    }
}