using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FciLuxor.Tests;

// Builds a UnitOfWork over in-memory databases, one pair per test.
public sealed class TestDatabase : IDisposable
{
    public EmployeesDbContext Employees { get; }
    public ProjectsDbContext Projects { get; }
    public UnitOfWork UnitOfWork { get; }

    public TestDatabase()
    {
        var name = Guid.NewGuid().ToString();
        Employees = new EmployeesDbContext(new DbContextOptionsBuilder<EmployeesDbContext>().UseInMemoryDatabase("employees-" + name).Options);
        Projects = new ProjectsDbContext(new DbContextOptionsBuilder<ProjectsDbContext>().UseInMemoryDatabase("projects-" + name).Options);

        UnitOfWork = new UnitOfWork(
            Employees,
            Projects,
            new AddressRepository(Employees),
            new TownRepository(Employees),
            new ProjectRepository(Projects),
            new EmployeeProjectRepository(Projects));
    }

    public void Dispose()
    {
        Employees.Dispose();
        Projects.Dispose();
    }
}
