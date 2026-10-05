# FCI Luxor Employees & Projects API

[![CI](https://github.com/99khalid/FCI2024/actions/workflows/ci.yml/badge.svg)](https://github.com/99khalid/FCI2024/actions/workflows/ci.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-8-512BD4)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)

A REST API for managing towns, addresses and projects, built with ASP.NET Core 8 using Clean Architecture, CQRS with MediatR, and the Repository + Unit of Work patterns over two SQL Server databases.

## Architecture

```
Domain/          Entities (Employee, Department, Project, Town, Address...) and enums
Application/     CQRS commands and queries with MediatR handlers, one folder per feature
Infrastructure/  EF Core DbContexts, configurations, migrations, repositories, Unit of Work
FciLuxor/        ASP.NET Core Web API: controllers, error handling middleware, Swagger
Tests/           xUnit tests for the handlers and the HTTP API
```

- **Two databases:** `EmployeesDB_API` (employees, towns, addresses) and `ProjectsDB_API` (projects, departments, assignments), each with its own `DbContext` and migrations.
- **Unit of Work:** repositories only track changes; `IUnitOfWork.CommitAsync` saves both databases.
- **Auditing and soft delete:** every entity inherits `BaseEntity`. `CreatedOn` / `UpdatedOn` are set automatically, deletes set `IsDeleted` instead of removing the row, and a global query filter hides deleted rows.
- **Errors:** missing records return `404` and rule violations (for example deleting a town that still has addresses) return `409`, both as `application/problem+json`.

## Endpoints

| Resource | Endpoints |
|---|---|
| Towns | `GET /api/towns`, `GET /api/towns/{id}`, `POST /api/towns`, `PUT /api/towns`, `DELETE /api/towns/{id}` |
| Addresses | `GET /api/addresses`, `GET /api/addresses/{id}`, `POST /api/addresses`, `PUT /api/addresses`, `DELETE /api/addresses/{id}` |
| Projects | `GET /api/projects`, `GET /api/projects/{id}`, `POST /api/projects`, `PUT /api/projects`, `DELETE /api/projects/{id}` |

Swagger UI is available at `/swagger` in development.

## Run it

Requirements: .NET 8 SDK and SQL Server (LocalDB works on Windows).

1. Set the connection strings in `FciLuxor/appsettings.json`, or keep secrets out of the repo with user secrets:
   ```bash
   cd FciLuxor
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:EmployeesDatabase" "Server=.;Database=EmployeesDB_API;User Id=sa;Password=<your-password>;TrustServerCertificate=True;"
   dotnet user-secrets set "ConnectionStrings:ProjectsDatabase" "Server=.;Database=ProjectsDB_API;User Id=sa;Password=<your-password>;TrustServerCertificate=True;"
   ```
2. Create the databases:
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update --project Infrastructure --startup-project FciLuxor --context EmployeesDbContext
   dotnet ef database update --project Infrastructure --startup-project FciLuxor --context ProjectsDbContext
   ```
3. Run the API:
   ```bash
   dotnet run --project FciLuxor
   ```

## Tests

```bash
dotnet test FciLuxorAPI.sln
```

The tests use the EF Core in-memory provider, so no SQL Server is needed.

## Authors

Built by [Khalid Hassan](https://github.com/99khalid) (FCI, 2021).
