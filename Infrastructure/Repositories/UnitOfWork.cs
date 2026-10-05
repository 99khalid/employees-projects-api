namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeesDbContext _employeesDbContext;
        private readonly ProjectsDbContext _projectsDbContext;

        public IAddressRepository Addresses { get; }
        public ITownRepository Towns { get; }
        public IProjectRepository Projects { get; }
        public IEmployeeProjectRepository EmployeeProjects { get; }

        public UnitOfWork(EmployeesDbContext employeesDbContext,
                          ProjectsDbContext projectsDbContext,
                          IAddressRepository addressRepository,
                          ITownRepository townRepository,
                          IProjectRepository projectRepository,
                          IEmployeeProjectRepository employeeProjectRepository)
        {
            _employeesDbContext = employeesDbContext;
            _projectsDbContext = projectsDbContext;
            Addresses = addressRepository;
            Towns = townRepository;
            Projects = projectRepository;
            EmployeeProjects = employeeProjectRepository;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var saved = await _employeesDbContext.SaveChangesAsync(cancellationToken);
            saved += await _projectsDbContext.SaveChangesAsync(cancellationToken);
            return saved;
        }
    }
}
