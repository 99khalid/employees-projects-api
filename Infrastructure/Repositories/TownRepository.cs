namespace Infrastructure.Repositories
{
    public class TownRepository : GenericRepository<Town>, ITownRepository
    {
        public TownRepository(EmployeesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
