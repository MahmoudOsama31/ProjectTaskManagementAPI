# Project & Task Management API

Small ASP.NET Core 9 Web API for authenticated project and task management (JWT, EF Core, SQL Server).

## Layout

| Folder | Project | Responsibility |
|--------|---------|------------------|
| `api` | `ProjectTaskManagement.Api` | HTTP: controllers, middleware, Swagger/JWT, API versioning |
| `application` | `ProjectTaskManagement.Application` | Services (with exception handling), DTOs, validators, entities |
| `infrastructure` | `ProjectTaskManagement.Infrastructure` | EF Core `ApplicationDbContext`, SQL Server, migrations |

**Domain model:** `User` is only for authentication. `Project` has many `TaskItem` (no link between `User` and `Project`). Access control is at the API layer via JWT `[Authorize]` on project/task endpoints.

## API versioning

All endpoints use URL versioning: **`/api/v1/...`**

| Method | Route |
|--------|-------|
| POST | `/api/v1/auth/register` |
| POST | `/api/v1/auth/login` |
| CRUD | `/api/v1/projects` |
| Tasks | `/api/v1/projects/{projectId}/tasks` |

Swagger shows a **v1** document in Development.

## Exception handling

- **Services:** `ServiceBase` + `ServiceExecutor` wrap operations; `AppException` is rethrown; DB/unexpected errors become consistent `AppException` responses.
- **API:** `ExceptionHandlingMiddleware` maps exceptions to JSON `{ "error": "..." }`.

## Run locally

1. Install [.NET 9 SDK](https://dotnet.microsoft.com/download).
2. Set `ConnectionStrings:DefaultConnection` in `api/appsettings.json`.
3. Create and apply migrations (removes `UserId` from `Projects` if you had an older schema):

```powershell
dotnet ef migrations add RemoveUserFromProject --project infrastructure/ProjectTaskManagement.Infrastructure.csproj --startup-project api/ProjectTaskManagement.Api.csproj --output-dir Persistence/Migrations
dotnet ef database update --project infrastructure/ProjectTaskManagement.Infrastructure.csproj --startup-project api/ProjectTaskManagement.Api.csproj
```

4. Run: `dotnet run --project api/ProjectTaskManagement.Api.csproj`
5. Swagger: `https://localhost:7203/swagger`

Ensure `Jwt:Key` is at least 32 characters.
