# Personal Finance Manager — Server

This folder contains the **server-side** .NET solution (`WebApp.slnx`).

## Projects

- `src/WebApp.Api`: ASP.NET Core Web API (host / startup)
- `src/WebApp.Data`: EF Core entities + `AppDbContext` + migrations
- `src/WebApp.Common`: shared enums/models used across server projects
- `src/UserManagement`: authentication & user management (services + registrations)
- `tests/UnitTests`: unit tests

## Run the API

```powershell
dotnet run --project src/WebApp.Api
```

Swagger UI (when `ASPNETCORE_ENVIRONMENT=Development`):

- `https://localhost:7195/swagger`
- `http://localhost:5130/swagger`

## Database migrations (SQL Server)

### Create a migration

```powershell
dotnet ef migrations add InitialCreate `
  --project src/WebApp.Data/WebApp.Data.csproj `
  --startup-project src/WebApp.Api/WebApp.Api.csproj `
  --output-dir Persistence/Migrations
```

### Apply migrations to DB

```powershell
dotnet ef database update `
  --project src/WebApp.Data/WebApp.Data.csproj `
  --startup-project src/WebApp.Api/WebApp.Api.csproj
```

### Notes

- Connection string is in `src/WebApp.Api/appsettings*.json` under `ConnectionStrings:DefaultConnection`.
- For LocalDB, ensure SQL Server LocalDB is installed.

