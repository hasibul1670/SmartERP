# SmartERP

Clean Architecture .NET 8 Web API (macOS + VS Code).

## Prerequisites

- .NET SDK 8 (already installed)
- VS Code extensions: C# Dev Kit, C#

Verify SDKs:

```bash
dotnet --list-sdks
```

## 1) Create solution + projects

Create a new solution folder:

```bash
mkdir -p ~/Projects/SmartERP
cd ~/Projects/SmartERP
```

Pin this solution to .NET 8 (important):

```bash
dotnet new globaljson --sdk-version 8.0.415
dotnet --version
```

Expected:

```
8.0.415
```

Create solution + folders:

```bash
mkdir -p src tests
dotnet new sln -n SmartERP
```

Create Clean Architecture projects:

- SmartERP.Domain (Entities)
- SmartERP.Application (Interfaces/Use cases)
- SmartERP.Infrastructure (DB/External services)
- SmartERP.Api (Web API)

```bash
dotnet new classlib -n SmartERP.Domain -o src/SmartERP.Domain
dotnet new classlib -n SmartERP.Application -o src/SmartERP.Application
dotnet new classlib -n SmartERP.Infrastructure -o src/SmartERP.Infrastructure
dotnet new webapi   -n SmartERP.Api -o src/SmartERP.Api
```

Add projects to the solution:

```bash
dotnet sln add src/SmartERP.Domain/SmartERP.Domain.csproj
dotnet sln add src/SmartERP.Application/SmartERP.Application.csproj
dotnet sln add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj
dotnet sln add src/SmartERP.Api/SmartERP.Api.csproj
```

Add project references (Clean Architecture rules):

```bash
# Application -> Domain
dotnet add src/SmartERP.Application/SmartERP.Application.csproj reference src/SmartERP.Domain/SmartERP.Domain.csproj

# Infrastructure -> Application + Domain
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj reference src/SmartERP.Application/SmartERP.Application.csproj
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj reference src/SmartERP.Domain/SmartERP.Domain.csproj

# API -> Application + Infrastructure
dotnet add src/SmartERP.Api/SmartERP.Api.csproj reference src/SmartERP.Application/SmartERP.Application.csproj
dotnet add src/SmartERP.Api/SmartERP.Api.csproj reference src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj
```

Cleanup default files (optional):

```bash
rm src/SmartERP.Domain/Class1.cs
rm src/SmartERP.Application/Class1.cs
rm src/SmartERP.Infrastructure/Class1.cs
```

## 2) Minimal User flow (in-memory)

Create a Domain entity:

`src/SmartERP.Domain/Entities/User.cs`

```csharp
namespace SmartERP.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

Create an Application interface:

`src/SmartERP.Application/Interfaces/IUserService.cs`

```csharp
using SmartERP.Domain.Entities;

namespace SmartERP.Application.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetAll();
}
```

Implement it in Infrastructure:

`src/SmartERP.Infrastructure/Services/UserService.cs`

```csharp
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespace SmartERP.Infrastructure.Services;

public class UserService : IUserService
{
    public IEnumerable<User> GetAll()
    {
        return new List<User>
        {
            new User { Id = 1, Name = "Hasibul" },
            new User { Id = 2, Name = "Test User" }
        };
    }
}
```

Register DI in API:

`src/SmartERP.Api/Program.cs`

```csharp
using SmartERP.Application.Interfaces;
using SmartERP.Infrastructure.Services;
```

```csharp
builder.Services.AddScoped<IUserService, UserService>();
```

Create an API controller:

`src/SmartERP.Api/Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespace SmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        return Ok(_userService.GetAll());
    }
}
```

Run the API:

```bash
cd src/SmartERP.Api
dotnet run
```

Open in browser:

- Swagger: `http://localhost:5297/swagger`
- API endpoint: `http://localhost:5297/api/users`

Open in VS Code (recommended):

```bash
cd ~/Projects/SmartERP
code .
```

Debug:

- Press `F5` → choose `.NET Core`

## Troubleshooting (quick fixes)

If it targets `net10.0` by mistake:

```bash
rg "<TargetFramework>" src
```

All should be:

```
<TargetFramework>net8.0</TargetFramework>
```

If packages mismatch:

- For .NET 8, use EF Core/Npgsql 8.x (not 10.x).

## Chapter: Add PostgreSQL + EF Core + Migrations + CRUD

Prerequisites:

- PostgreSQL installed locally (or via Docker)
- A database created (example name: `SmartERP`)
- You know your username/password

Optional: create DB via psql:

```bash
createdb SmartERP
```

### 1) Add EF Core packages to Infrastructure

From solution root:

```bash
cd ~/Projects/SmartERP

dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.11
```

Install EF CLI tool (one time):

```bash
dotnet tool install --global dotnet-ef
dotnet ef --version
```

### 2) Create DbContext in Infrastructure

`src/SmartERP.Infrastructure/Persistence/AppDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SmartERP.Domain.Entities;

namespace SmartERP.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
```

### 3) Add connection string in API

Edit `src/SmartERP.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "SmartERP": "Host=localhost;Port=5432;Database=SmartERP;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

Replace `Username` and `Password`.

### 4) Register DbContext in API

`src/SmartERP.Api/Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SmartERP.Infrastructure.Persistence;
```

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));
```

### 5) Create the first migration

```bash
cd ~/Projects/SmartERP

dotnet ef migrations add InitialCreate \
  --project src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj \
  --startup-project src/SmartERP.Api/SmartERP.Api.csproj \
  --output-dir Persistence/Migrations
```

Migrations are generated in:

`src/SmartERP.Infrastructure/Persistence/Migrations`

### 6) Apply migration to PostgreSQL

```bash
dotnet ef database update \
  --project src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj \
  --startup-project src/SmartERP.Api/SmartERP.Api.csproj
```

Now PostgreSQL has a `Users` table.

### 7) Update IUserService to async CRUD

`src/SmartERP.Application/Interfaces/IUserService.cs`

```csharp
using SmartERP.Domain.Entities;

namespace SmartERP.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
}
```

### 8) Implement CRUD in Infrastructure using EF Core

`src/SmartERP.Infrastructure/Services/UserService.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;
using SmartERP.Infrastructure.Persistence;

namespace SmartERP.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllAsync()
        => await _db.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(int id, User user)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return false;

        existing.Name = user.Name;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return false;

        _db.Users.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
```

### 9) Ensure DI registration is present

`src/SmartERP.Api/Program.cs`

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));

builder.Services.AddScoped<IUserService, UserService>();
```

### 10) Update UsersController to CRUD

`src/SmartERP.Api/Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespace SmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users)
    {
        _users = users;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
        => Ok(await _users.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _users.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        var created = await _users.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        var ok = await _users.UpdateAsync(id, user);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _users.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
```

### 11) Run the API

```bash
cd ~/Projects/SmartERP/src/SmartERP.Api
dotnet run
```

Open Swagger:

- `http://localhost:PORT/swagger`

Test endpoints:

```
GET /api/users
POST /api/users
{"name":"Hasibul"}

PUT /api/users/1
DELETE /api/users/1
```

### Troubleshooting

A) “SDK does not support targeting net10.0”

```xml
<TargetFramework>net8.0</TargetFramework>
```

B) NU1202 package not compatible with net8.0  
Remove EF/Npgsql 10.x and install 8.x.

C) Migrations error: can’t create DbContext

Make sure:

- Connection string exists in `src/SmartERP.Api/appsettings.json`
- `builder.Services.AddDbContext<AppDbContext>(...)` is in `src/SmartERP.Api/Program.cs`

## Chapter: Repository Pattern + DTOs + Validation (Simple)

This chapter will:

- Add a Repository interface in Application
- Implement it in Infrastructure using EF Core
- Add DTOs so API doesn’t expose Entity directly
- Add FluentValidation for clean request validation

Goal structure:

- Domain: Entities (User)
- Application: Interfaces + DTOs + Validators
- Infrastructure: EF Core DbContext + Repository implementations
- API: Controllers use DTOs and call repository

### 1) Create DTOs in Application

Create folder:

```bash
mkdir -p src/SmartERP.Application/Users
```

`src/SmartERP.Application/Users/UserDto.cs`

```csharp
namespace SmartERP.Application.Users;

public record UserDto(int Id, string Name);
```

`src/SmartERP.Application/Users/CreateUserRequest.cs`

```csharp
namespace SmartERP.Application.Users;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
}
```

`src/SmartERP.Application/Users/UpdateUserRequest.cs`

```csharp
namespace SmartERP.Application.Users;

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;
}
```

### 2) Add Repository interface in Application

Create folder:

```bash
mkdir -p src/SmartERP.Application/Users/Repositories
```

`src/SmartERP.Application/Users/Repositories/IUserRepository.cs`

```csharp
using SmartERP.Domain.Entities;

namespace SmartERP.Application.Users.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}
```

### 3) Implement Repository in Infrastructure

Create folder:

```bash
mkdir -p src/SmartERP.Infrastructure/Users/Repositories
```

`src/SmartERP.Infrastructure/Users/Repositories/UserRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SmartERP.Application.Users.Repositories;
using SmartERP.Domain.Entities;
using SmartERP.Infrastructure.Persistence;

namespace SmartERP.Infrastructure.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllAsync()
        => await _db.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _db.Users.Update(user);
        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return false;

        _db.Users.Remove(existing);
        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }
}
```

### 4) Register Repository in API DI

`src/SmartERP.Api/Program.cs`

```csharp
using SmartERP.Application.Users.Repositories;
using SmartERP.Infrastructure.Users.Repositories;
```

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();
```

### 5) Add FluentValidation (simple)

From solution root:

```bash
dotnet add src/SmartERP.Api/SmartERP.Api.csproj package FluentValidation.AspNetCore --version 11.10.0
```

If version install fails, remove `--version ...` and install latest.

### 6) Create validators in Application

Create folder:

```bash
mkdir -p src/SmartERP.Application/Users/Validation
```

`src/SmartERP.Application/Users/Validation/CreateUserRequestValidator.cs`

```csharp
using FluentValidation;

namespace SmartERP.Application.Users.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
```

`src/SmartERP.Application/Users/Validation/UpdateUserRequestValidator.cs`

```csharp
using FluentValidation;

namespace SmartERP.Application.Users.Validation;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
```

### 7) Register FluentValidation in API

`src/SmartERP.Api/Program.cs`

```csharp
using FluentValidation;
using FluentValidation.AspNetCore;
using SmartERP.Application.Users.Validation;
```

```csharp
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
```

### 8) Update Controller to use DTOs + Validation + Repository

`src/SmartERP.Api/Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Users;
using SmartERP.Application.Users.Repositories;
using SmartERP.Domain.Entities;

namespace SmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;

    public UsersController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _repo.GetAllAsync();
        var dtos = users.Select(u => new UserDto(u.Id, u.Name)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) return NotFound();

        return Ok(new UserDto(user.Id, user.Name));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request)
    {
        // FluentValidation runs automatically before this method due to AddFluentValidationAutoValidation()
        var user = new User { Name = request.Name };
        var created = await _repo.AddAsync(user);

        var dto = new UserDto(created.Id, created.Name);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        var updated = new User { Id = existing.Id, Name = request.Name };
        var ok = await _repo.UpdateAsync(updated);

        return ok ? NoContent() : StatusCode(500);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
```

### 9) Run and test

```bash
cd ~/Projects/SmartERP/src/SmartERP.Api
dotnet run
```

In Swagger test:

```
POST /api/users
{"name":""}
```

Should return `400` with validation errors.

```
POST /api/users
{"name":"Hasibul"}
```

Should create user.
