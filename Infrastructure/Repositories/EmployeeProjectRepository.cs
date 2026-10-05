namespace Infrastructure.Repositories
{
    public class EmployeeProjectRepository : GenericRepository<EmployeesProjects>, IEmployeeProjectRepository
    {
        public EmployeeProjectRepository(ProjectsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
