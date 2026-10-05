namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EmployeesDatabase"),
                    sql => sql.MigrationsAssembly("Infrastructure")));

            services.AddDbContext<ProjectsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProjectsDatabase"),
                    sql => sql.MigrationsAssembly("Infrastructure")));

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ITownRepository, TownRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
