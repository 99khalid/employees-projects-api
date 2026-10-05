using System.Net;
using System.Net.Http.Json;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FciLuxor.Tests;

public class ApiTests : IClassFixture<ApiTests.InMemoryApiFactory>
{
    public class InMemoryApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _name = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<EmployeesDbContext>>();
                services.RemoveAll<DbContextOptions<ProjectsDbContext>>();
                services.AddDbContext<EmployeesDbContext>(o => o.UseInMemoryDatabase("employees-" + _name));
                services.AddDbContext<ProjectsDbContext>(o => o.UseInMemoryDatabase("projects-" + _name));
            });
        }
    }

    private record IdResponse(Guid Id);
    private record TownResponse(Guid TownID, string Name);

    private readonly HttpClient _client;

    public ApiTests(InMemoryApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostTown_ThenGetById_ReturnsTheTown()
    {
        var create = await _client.PostAsJsonAsync("/api/towns", new { name = "Luxor" });
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        var created = await create.Content.ReadFromJsonAsync<IdResponse>();

        var town = await _client.GetFromJsonAsync<TownResponse>($"/api/towns/{created!.Id}");

        Assert.Equal("Luxor", town!.Name);
    }

    [Fact]
    public async Task GetUnknownTown_Returns404()
    {
        var response = await _client.GetAsync($"/api/towns/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUnknownProject_Returns404Problem()
    {
        var response = await _client.DeleteAsync($"/api/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}

internal static class ServiceCollectionExtensions
{
    public static void RemoveAll<T>(this IServiceCollection services)
    {
        foreach (var descriptor in services.Where(d => d.ServiceType == typeof(T)).ToList())
        {
            services.Remove(descriptor);
        }
    }
}
