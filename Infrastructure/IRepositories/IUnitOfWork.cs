namespace Infrastructure.IRepositories
{
    public interface IUnitOfWork
    {
        IAddressRepository Addresses { get; }
        ITownRepository Towns { get; }
        IProjectRepository Projects { get; }
        IEmployeeProjectRepository EmployeeProjects { get; }

        // Saves the pending changes of both databases.
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
