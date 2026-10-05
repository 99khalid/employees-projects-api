using Application.Features.Addresses.Commands.CreateAddress;
using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Commands.DeleteProject;
using Application.Features.Projects.Commands.UpdateProject;
using Application.Features.Towns.Commands.CreateTown;
using Application.Features.Towns.Commands.DeleteTown;
using Application.Features.Towns.Queries.GetAllTowns;
using Application.Features.Towns.Queries.GetTownById;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FciLuxor.Tests;

public class HandlerTests : IDisposable
{
    private readonly TestDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private Task<Guid> CreateTown(string name) =>
        new CreateTownHandler(_db.UnitOfWork).Handle(new CreateTownRequest { Name = name }, default);

    [Fact]
    public async Task CreateTown_ReturnsIdAndCanBeReadBack()
    {
        var id = await CreateTown("Luxor");

        var town = await new GetTownByIdHandler(_db.UnitOfWork).Handle(new GetTownByIdRequest { TownID = id }, default);

        Assert.NotNull(town);
        Assert.Equal("Luxor", town!.Name);
        Assert.NotEqual(default, town.CreatedOn);
    }

    [Fact]
    public async Task CreateAddress_WithUnknownTown_Throws()
    {
        var handler = new CreateAddressHandler(_db.UnitOfWork);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new CreateAddressRequest { AddressText = "1 Nile St", TownID = Guid.NewGuid() }, default));
    }

    [Fact]
    public async Task DeleteTown_WithAddresses_IsRejected()
    {
        var townId = await CreateTown("Aswan");
        await new CreateAddressHandler(_db.UnitOfWork).Handle(new CreateAddressRequest { AddressText = "1 Nile St", TownID = townId }, default);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new DeleteTownHandler(_db.UnitOfWork).Handle(new DeleteTownRequest { TownID = townId }, default));
    }

    [Fact]
    public async Task DeleteTown_SoftDeletesAndHidesTheTown()
    {
        var townId = await CreateTown("Qena");

        await new DeleteTownHandler(_db.UnitOfWork).Handle(new DeleteTownRequest { TownID = townId }, default);

        var towns = await new GetAllTownsHandler(_db.UnitOfWork).Handle(new GetAllTownsRequest(), default);
        Assert.Empty(towns);

        var row = await _db.Employees.Towns.IgnoreQueryFilters().SingleAsync(t => t.TownID == townId);
        Assert.True(row.IsDeleted);
    }

    [Fact]
    public async Task DeleteTown_Unknown_Throws()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            new DeleteTownHandler(_db.UnitOfWork).Handle(new DeleteTownRequest { TownID = Guid.NewGuid() }, default));
    }

    [Fact]
    public async Task UpdateProject_ChangesFields()
    {
        var id = await new CreateProjectHandler(_db.UnitOfWork).Handle(
            new CreateProjectRequest { Name = "Old", Description = "d", StartDate = new DateTime(2024, 1, 1) }, default);

        await new UpdateProjectHandler(_db.UnitOfWork).Handle(
            new UpdateProjectRequest { ProjectID = id, Name = "New", Description = "d2", StartDate = new DateTime(2024, 2, 1) }, default);

        var project = await _db.Projects.Projects.AsNoTracking().SingleAsync(p => p.ProjectID == id);
        Assert.Equal("New", project.Name);
        Assert.NotNull(project.UpdatedOn);
    }

    [Fact]
    public async Task DeleteProject_WithEmployeesAssigned_IsRejected()
    {
        var id = await new CreateProjectHandler(_db.UnitOfWork).Handle(
            new CreateProjectRequest { Name = "P", Description = "d", StartDate = DateTime.Today }, default);
        _db.Projects.EmployeesProjects.Add(new EmployeesProjects { EmployeeID = Guid.NewGuid(), ProjectID = id });
        await _db.Projects.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new DeleteProjectHandler(_db.UnitOfWork).Handle(new DeleteProjectRequest { ProjectID = id }, default));
    }
}
