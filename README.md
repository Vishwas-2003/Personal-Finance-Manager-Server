# Personal Finance Manager — Server

REST API backend for the Personal Finance Manager application. Tracks users, expenses, income, budgets, and category-based financial summaries with JWT authentication and refresh tokens.

This folder contains the **server-side** .NET solution (`WebApp.slnx`).

## Features

- **Authentication** — Register, login, and refresh access tokens (`/api/Auth`)
- **User profile** — Get user details by id (no password in response)
- **Expenses** — Add, list with optional filters (category, date range), delete
- **Income** — Add, list, delete
- **Budgets** — Add, list, delete
- **Categories** — List all categories (with types)
- **Summaries**
  - Income summary — totals grouped by category hierarchy
  - Expense summary — totals grouped by category hierarchy
  - **Balance summary** — credit sheet (income) and debit sheet (expenses) with `TotalCredit`, `TotalDebit`, and net `Balance` (credit − debit); optional date range filter on expenses and income
- **Session handling** — Expired or invalid JWT returns `401` with `SESSION_EXPIRED`; refresh failure uses the same error shape

## Tech stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core + SQL Server
- JWT Bearer authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- AutoMapper

## Architecture

Layered design:

```
Controllers (WebApp.Api)
    → Services (WebApp.Api)
        → Repositories (WebApp.Data)
            → EF Core / AppDbContext (WebApp.Data)
```

Shared DTOs and API models live in `WebApp.Common`. Authentication services and JWT generation live in `UserManagement`.

## Projects

| Project | Path | Purpose |
|---------|------|---------|
| `WebApp.Api` | `src/WebApp.Api` | ASP.NET Core Web API (host, controllers, services, DI) |
| `WebApp.Data` | `src/WebApp.Data` | EF Core entities, `AppDbContext`, repositories, migrations |
| `WebApp.Common` | `src/WebApp.Common` | Shared models, filters, API error contracts, validation |
| `UserManagement` | `src/UserManagement` | Auth services, JWT token service, user registration/login |
| `UnitTests` | `tests/UnitTests` | Unit tests |

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) (or SDK matching `TargetFramework` in project files)
- SQL Server, SQL Server Express, or LocalDB
- [EF Core tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for migrations: `dotnet tool install --global dotnet-ef`

## Configuration

Edit `src/WebApp.Api/appsettings.json` (and `appsettings.Development.json` for local overrides):

| Section | Keys | Description |
|---------|------|-------------|
| `ConnectionStrings` | `DefaultConnection` | SQL Server connection string |
| `Jwt` | `Issuer`, `Audience`, `Secret` | Token validation and signing |
| `Jwt` | `AccessTokenMinutes` | Access token lifetime (default: 30) |
| `Jwt` | `RefreshTokenDays` | Refresh token lifetime (default: 7) |

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PersonalFinanceManager;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Jwt": {
    "Issuer": "WebApp.Api",
    "Audience": "WebApp.Client",
    "Secret": "your-signing-secret-min-32-chars",
    "AccessTokenMinutes": 30,
    "RefreshTokenDays": 7
  }
}
```

> **Security:** Do not commit production secrets. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables for `Jwt:Secret` and connection strings in deployed environments.

## Run the API

From the `Server` folder:

```powershell
dotnet restore
dotnet run --project src/WebApp.Api
```

Default URLs (see `src/WebApp.Api/Properties/launchSettings.json`):

| Profile | URL |
|---------|-----|
| HTTPS | `https://localhost:7195` |
| HTTP | `http://localhost:5130` |

## Authentication

| Endpoint | Auth | Description |
|----------|------|-------------|
| `POST /api/Auth/register` | Anonymous | Create account |
| `POST /api/Auth/login` | Anonymous | Login; returns tokens |
| `POST /api/Auth/refresh` | Anonymous | Exchange refresh token for new tokens |

All other controllers require header:

```http
Authorization: Bearer {accessToken}
```

### Register — `POST /api/Auth/register`

**Body:**

```json
{
  "name": "Jane Doe",
  "mobileNumber": "+919876543210",
  "age": 30,
  "address": "123 Main St",
  "email": "jane@example.com",
  "password": "YourSecureP@ss1"
}
```

Password must satisfy the server password policy (`PasswordPolicy` validation attribute).

### Login — `POST /api/Auth/login`

**Body:**

```json
{
  "email": "jane@example.com",
  "password": "YourSecureP@ss1"
}
```

### Refresh — `POST /api/Auth/refresh`

**Body:**

```json
{
  "refreshToken": "your-refresh-token"
}
```

### JWT challenge (expired access token)

When a protected endpoint is called with a missing or expired access token, the API returns `401` with the same `SESSION_EXPIRED` JSON shape (via JWT bearer `OnChallenge`).

## API reference

Protected endpoints require `Authorization: Bearer {accessToken}` unless noted.

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/User/get/{userId}` | User profile |
| POST | `/api/Expense/add` | Add expense |
| GET | `/api/Expense/get/{userId}` | List expenses |
| DELETE | `/api/Expense/delete/{expenseId}` | Delete expense |
| POST | `/api/Income/add` | Add income |
| GET | `/api/Income/get/{userId}` | List income |
| DELETE | `/api/Income/delete/{incomeId}` | Delete income |
| POST | `/api/Budget/add` | Add budget |
| GET | `/api/Budget/get/{userId}` | List budgets |
| DELETE | `/api/Budget/delete/{budgetId}` | Delete budget |
| GET | `/api/Category/get` | List categories |
| GET | `/api/Summary/income/{userId}` | Income summary |
| GET | `/api/Summary/expense/{userId}` | Expense summary |
| GET | `/api/Summary/balance/{userId}` | Balance summary |

## Database migrations (SQL Server)

Migrations are stored under `src/WebApp.Data/Persistence/Migrations/`.

Existing migrations in this repo:

- `20260506085651_InitialMigration`
- `20260506111815_UpdateUserEntity`

### Create a migration

```powershell
dotnet ef migrations add InitialCreate `
  --project src/WebApp.Data/WebApp.Data.csproj `
  --startup-project src/WebApp.Api/WebApp.Api.csproj `
  --output-dir Persistence/Migrations
```

Replace `InitialCreate` with a descriptive name when adding new schema changes (e.g. `AddBudgetTable`).

### Apply migrations to DB

```powershell
dotnet ef database update `
  --project src/WebApp.Data/WebApp.Data.csproj `
  --startup-project src/WebApp.Api/WebApp.Api.csproj
```

### Notes

- Connection string is in `src/WebApp.Api/appsettings*.json` under `ConnectionStrings:DefaultConnection`.
- For LocalDB, ensure SQL Server LocalDB is installed.
- Run `database update` before first API run on a new machine or after pulling new migrations.

## Run tests

```powershell
dotnet test tests/UnitTests
```

Business-logic coverage (WebApp.Api, UserManagement, WebApp.Common; excludes WebApp.Data and startup/DI wiring) is collected automatically and must stay at or above **85%** line coverage. Stop the running API first if the build reports locked files under `src/WebApp.Api/bin`.

Unit test projects (`tests/UnitTests`):

| Test class | Coverage |
|------------|----------|
| `AuthServiceTests` | Register, login, refresh (valid, expired, unknown token) |
| `UserServiceTests` / `UserControllerTests` | User profile by id |
| `IncomeServiceTests` / `IncomeControllerTests` | Income CRUD |
| `BudgetServiceTests` / `BudgetControllerTests` | Budget CRUD |
| `ExpenseServiceTests` / `ExpenseControllerTests` | Expense CRUD and list filters |
| `SummaryServiceTests` / `SummaryControllerTests` | Income/expense summaries and balance summary |
| `SummaryHierarchyUtilityTests` | Category hierarchy grouping for summaries |
| `AuthControllerTests` / `CategoryControllerTests` | Auth and category API endpoints |
| `JwtTokenServiceTests` | JWT and refresh token generation |
| `ControllerExceptionHandlerTests` | API exception mapping |
| `KeywordFilterUtilityTests` / `TransactionDateValidatorTests` | Shared filter and date validation utilities |

> Stop the running API process if the build reports locked files under `src/WebApp.Api/bin`.

## Related client

The console client for this API lives in a separate repository/folder (`Personal-Finance-Manager-Client`). Configure its `Api:BaseUrl` to match this API (e.g. `https://localhost:7195`).
