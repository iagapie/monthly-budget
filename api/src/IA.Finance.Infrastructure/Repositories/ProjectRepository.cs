using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace IA.Finance.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly FinanceContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(FinanceContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        public void Add(Project project) => _context.Projects.Add(project);

        public void Update(Project project) => _context.Entry(project).State = EntityState.Modified;
        
        public void Remove(Project project) => _context.Entry(project).State = EntityState.Deleted;

        public async Task<Project> FindById(long projectId) =>
            await _context.Projects.FindAsync(projectId).ConfigureAwait(false);

        public async Task<IEnumerable<Project>> FindByOwnerId(long ownerId) =>
            await _context.Projects
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync()
                .ConfigureAwait(false);
    }
}