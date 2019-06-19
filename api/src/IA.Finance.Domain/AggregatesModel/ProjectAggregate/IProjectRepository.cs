using System.Collections.Generic;
using System.Threading.Tasks;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.ProjectAggregate
{
    public interface IProjectRepository : IRepository<Project>
    {
        void Add(Project project);
        
        void Update(Project project);

        void Remove(Project project);
        
        Task<Project> FindById(long projectId);

        Task<IEnumerable<Project>> FindByOwnerId(long ownerId);
    }
}