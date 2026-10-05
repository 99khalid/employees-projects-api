namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ProjectsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
