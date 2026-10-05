namespace Infrastructure.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(EmployeesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
