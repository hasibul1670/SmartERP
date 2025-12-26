Create a Project with .NET
Clean Architecture .NET 8 Web API (macOS + VS Code)
Prerequisites
Install .NET SDK 8 (you already have it).
VS Code Extensions:
C# Dev Kit
C#
Verify SDKs:

dotnet --list-sdks

1) Create a new solution folder
mkdir -p ~/Projects/SmartERP
cd ~/Projects/SmartERP

Pin this solution to .NET 8 (important)
dotnet new globaljson --sdk-version 8.0.415
dotnet --version

Expected:

8.0.415

2) Create solution + folders
mkdir -p src tests
dotnet new sln -n SmartERP

3) Create Clean Architecture projects
We will create 4 projects:

SmartERP.Domain (Entities)
SmartERP.Application (Interfaces/Use cases)
SmartERP.Infrastructure (DB/External services)
SmartERP.Api (Web API)
Run:

dotnet new classlib -n SmartERP.Domain -o src/SmartERP.Domain
dotnet new classlib -n SmartERP.Application -o src/SmartERP.Application
dotnet new classlib -n SmartERP.Infrastructure -o src/SmartERP.Infrastructure
dotnet new webapi   -n SmartERP.Api -o src/SmartERP.Api

4) Add projects to the solution
dotnet sln add src/SmartERP.Domain/SmartERP.Domain.csproj
dotnet sln add src/SmartERP.Application/SmartERP.Application.csproj
dotnet sln add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj
dotnet sln add src/SmartERP.Api/SmartERP.Api.csproj

5) Add project references (Clean Architecture rules)
# Application -> Domain
dotnet add src/SmartERP.Application/SmartERP.Application.csproj reference src/SmartERP.Domain/SmartERP.Domain.csproj

# Infrastructure -> Application + Domain
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj reference src/SmartERP.Application/SmartERP.Application.csproj
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj reference src/SmartERP.Domain/SmartERP.Domain.csproj

# API -> Application + Infrastructure
dotnet add src/SmartERP.Api/SmartERP.Api.csproj reference src/SmartERP.Application/SmartERP.Application.csproj
dotnet add src/SmartERP.Api/SmartERP.Api.csproj reference src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj

6) Cleanup default files (optional)
rm src/SmartERP.Domain/Class1.cs
rm src/SmartERP.Application/Class1.cs
rm src/SmartERP.Infrastructure/Class1.cs

7) Create a Domain entity: User
Create file:

src/SmartERP.Domain/Entities/User.cs

namespaceSmartERP.Domain.Entities;

publicclassUser
{
publicint Id {get;set; }
publicstring Name {get;set; } =string.Empty;
}

8) Create an Application interface: IUserService
Create file:

src/SmartERP.Application/Interfaces/IUserService.cs

using SmartERP.Domain.Entities;

namespaceSmartERP.Application.Interfaces;

publicinterfaceIUserService
{
IEnumerable<User> GetAll();
}

9) Implement it in Infrastructure: UserService
Create file:

src/SmartERP.Infrastructure/Services/UserService.cs

using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespaceSmartERP.Infrastructure.Services;

publicclassUserService :IUserService
{
public IEnumerable<User>GetAll()
    {
returnnew List<User>
        {
new User { Id =1, Name ="Hasibul" },
new User { Id =2, Name ="Test User" }
        };
    }
}

10) Register DI in API (Program.cs)
Open:

src/SmartERP.Api/Program.cs

Add these using lines at the top:

using SmartERP.Application.Interfaces;
using SmartERP.Infrastructure.Services;

Then before var app = builder.Build(); add:

builder.Services.AddScoped<IUserService, UserService>();

11) Create an API Controller
Create file:

src/SmartERP.Api/Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespaceSmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
publicclassUsersController :ControllerBase
{
privatereadonly IUserService _userService;

publicUsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
public ActionResult<IEnumerable<User>> Get()
    {
return Ok(_userService.GetAll());
    }
}

12) Run the API
cd src/SmartERP.Api
dotnet run

You’ll see something like:

Now listeningon: http://localhost:5297

Open in browser:

Swagger: http://localhost:5297/swagger
API endpoint: http://localhost:5297/api/users
13) Open in VS Code (recommended)
From the solution root:

cd ~/Projects/SmartERP
code .

Debug:

Press F5 → choose .NET Core
Common issues (quick fixes)
If it targets net10.0 by mistake
Check:

grep -R"<TargetFramework>" -n src

All should be:

<TargetFramework>net8.0</TargetFramework>

If packages mismatch
When using .NET 8, use EF Core/Npgsql 8.x, not 10.x.

Chapter: Add PostgreSQL + EF Core + Migrations + CRUD
Prerequisites
PostgreSQL installed locally (or via Docker)
A database created (example name: SmartERP)
You know your username/password
Optional: create DB via psql:

createdb SmartERP
1) Add EF Core packages to Infrastructure
From solution root:

cd ~/Projects/SmartERP

dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
dotnet add src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.11

Also install EF CLI tool (one time):

dotnet tool install --global dotnet-ef

Verify:

dotnet ef --version

2) Create DbContext in Infrastructure
Create file:

src/SmartERP.Infrastructure/Persistence/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using SmartERP.Domain.Entities;

namespaceSmartERP.Infrastructure.Persistence;

publicclassAppDbContext :DbContext
{
publicAppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

public DbSet<User> Users => Set<User>();
}

3) Add connection string in API
Edit:

src/SmartERP.Api/appsettings.json

Add this:

{
"ConnectionStrings":{
"SmartERP":"Host=localhost;Port=5432;Database=SmartERP;Username=postgres;Password=YOUR_PASSWORD"
}
}

Replace:

Username
Password
4) Register DbContext in API Program.cs
Open:

src/SmartERP.Api/Program.cs

Add using at top:

using Microsoft.EntityFrameworkCore;
using SmartERP.Infrastructure.Persistence;

Then before var app = builder.Build(); add:

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));

✅ Now the API can connect to PostgreSQL.

5) Create the first migration
From solution root:

cd ~/Projects/SmartERP

dotnet ef migrations add InitialCreate \
  --project src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj \
  --startup-project src/SmartERP.Api/SmartERP.Api.csproj \
  --output-dir Persistence/Migrations

This generates migrations inside:

src/SmartERP.Infrastructure/Persistence/Migrations

6) Apply migration to PostgreSQL
dotnet ef database update \
  --project src/SmartERP.Infrastructure/SmartERP.Infrastructure.csproj \
  --startup-project src/SmartERP.Api/SmartERP.Api.csproj

✅ Now PostgreSQL has a Users table.

7) Make User entity EF-friendly (optional but recommended)
Your User.cs is already okay. Keep it simple:

src/SmartERP.Domain/Entities/User.cs

namespaceSmartERP.Domain.Entities;

publicclassUser
{
publicint Id {get;set; }
publicstring Name {get;set; } =string.Empty;
}

8) Update IUserService to async CRUD
Edit:

src/SmartERP.Application/Interfaces/IUserService.cs

using SmartERP.Domain.Entities;

namespaceSmartERP.Application.Interfaces;

publicinterfaceIUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
Task<User> CreateAsync(User user);
Task<bool>UpdateAsync(int id, User user);
Task<bool>DeleteAsync(int id);
}

9) Implement CRUD in Infrastructure using EF Core
Edit/create:

src/SmartERP.Infrastructure/Services/UserService.cs

using Microsoft.EntityFrameworkCore;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;
using SmartERP.Infrastructure.Persistence;

namespaceSmartERP.Infrastructure.Services;

publicclassUserService :IUserService
{
privatereadonly AppDbContext _db;

publicUserService(AppDbContext db)
    {
        _db = db;
    }

publicasync Task<List<User>> GetAllAsync()
        =>await _db.Users.AsNoTracking().ToListAsync();

publicasync Task<User?> GetByIdAsync(int id)
        =>await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

publicasync Task<User>CreateAsync(User user)
    {
        _db.Users.Add(user);
await _db.SaveChangesAsync();
return user;
    }

publicasync Task<bool>UpdateAsync(int id, User user)
    {
var existing =await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
if (existingisnull)returnfalse;

        existing.Name = user.Name;
await _db.SaveChangesAsync();
returntrue;
    }

publicasync Task<bool>DeleteAsync(int id)
    {
var existing =await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
if (existingisnull)returnfalse;

        _db.Users.Remove(existing);
await _db.SaveChangesAsync();
returntrue;
    }
}

10) Ensure DI registration is present
In Program.cs make sure you have both:

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));

builder.Services.AddScoped<IUserService, UserService>();
11) Update UsersController to real CRUD
Replace your controller with:

src/SmartERP.Api/Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Interfaces;
using SmartERP.Domain.Entities;

namespaceSmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
publicclassUsersController :ControllerBase
{
privatereadonly IUserService _users;

publicUsersController(IUserService users)
    {
        _users = users;
    }

    [HttpGet]
publicasync Task<ActionResult<List<User>>> GetAll()
        => Ok(await _users.GetAllAsync());

    [HttpGet("{id:int}")]
publicasync Task<ActionResult<User>> GetById(int id)
    {
var user =await _users.GetByIdAsync(id);
return userisnull ? NotFound() : Ok(user);
    }

    [HttpPost]
publicasync Task<ActionResult<User>> Create(User user)
    {
var created =await _users.CreateAsync(user);
return CreatedAtAction(nameof(GetById),new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
publicasync Task<IActionResult>Update(int id, User user)
    {
var ok =await _users.UpdateAsync(id, user);
return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
publicasync Task<IActionResult>Delete(int id)
    {
var ok =await _users.DeleteAsync(id);
return ok ? NoContent() : NotFound();
    }
}

12) Run the API
cd ~/Projects/SmartERP/src/SmartERP.Api
dotnet run

Open Swagger:

http://localhost:PORT/swagger
Test endpoints:

GET /api/users
POST /api/users with JSON:
{"name":"Hasibul"}

PUT /api/users/1
DELETE /api/users/1
Troubleshooting
A) “SDK does not support targeting net10.0”
Your csproj targets net10.0. Fix to:

<TargetFramework>net8.0</TargetFramework>

B) NU1202 package not compatible with net8.0
You installed EF/Npgsql 10.x. Remove them and install 8.x.

C) Migrations error: can’t create DbContext
Make sure:

Connection string exists in API appsettings.json
builder.Services.AddDbContext<AppDbContext>(...) is in Program.cs
This chapter will:

Add a Repository interface in Application
Implement it in Infrastructure using EF Core
Add DTOs so API doesn’t expose Entity directly
Add FluentValidation for clean request validation
Chapter: Repository Pattern + DTOs + Validation (Simple)
Goal structure
Domain: Entities (User)
Application: Interfaces + DTOs + Validators
Infrastructure: EF Core DbContext + Repository implementations
API: Controllers use DTOs and call repository
1) Create DTOs in Application
Create folder:

mkdir -p src/SmartERP.Application/Users

Create UserDto.cs
src/SmartERP.Application/Users/UserDto.cs

namespaceSmartERP.Application.Users;

publicrecordUserDto(int Id,string Name);

Create CreateUserRequest.cs
src/SmartERP.Application/Users/CreateUserRequest.cs

namespaceSmartERP.Application.Users;

publicclassCreateUserRequest
{
publicstring Name {get;set; } =string.Empty;
}

Create UpdateUserRequest.cs
src/SmartERP.Application/Users/UpdateUserRequest.cs

namespaceSmartERP.Application.Users;

publicclassUpdateUserRequest
{
publicstring Name {get;set; } =string.Empty;
}

2) Add Repository interface in Application
Create folder:

mkdir -p src/SmartERP.Application/Users/Repositories

Create interface:

src/SmartERP.Application/Users/Repositories/IUserRepository.cs

using SmartERP.Domain.Entities;

namespaceSmartERP.Application.Users.Repositories;

publicinterfaceIUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
Task<User> AddAsync(User user);
Task<bool>UpdateAsync(User user);
Task<bool>DeleteAsync(int id);
}

3) Implement Repository in Infrastructure
Create folder:

mkdir -p src/SmartERP.Infrastructure/Users/Repositories

Create file:

src/SmartERP.Infrastructure/Users/Repositories/UserRepository.cs

using Microsoft.EntityFrameworkCore;
using SmartERP.Application.Users.Repositories;
using SmartERP.Domain.Entities;
using SmartERP.Infrastructure.Persistence;

namespaceSmartERP.Infrastructure.Users.Repositories;

publicclassUserRepository :IUserRepository
{
privatereadonly AppDbContext _db;

publicUserRepository(AppDbContext db)
    {
        _db = db;
    }

publicasync Task<List<User>> GetAllAsync()
        =>await _db.Users.AsNoTracking().ToListAsync();

publicasync Task<User?> GetByIdAsync(int id)
        =>await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

publicasync Task<User>AddAsync(User user)
    {
        _db.Users.Add(user);
await _db.SaveChangesAsync();
return user;
    }

publicasync Task<bool>UpdateAsync(User user)
    {
        _db.Users.Update(user);
var changes =await _db.SaveChangesAsync();
return changes >0;
    }

publicasync Task<bool>DeleteAsync(int id)
    {
var existing =await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
if (existingisnull)returnfalse;

        _db.Users.Remove(existing);
var changes =await _db.SaveChangesAsync();
return changes >0;
    }
}

4) Register Repository in API DI
Edit src/SmartERP.Api/Program.cs and add:

using SmartERP.Application.Users.Repositories;
using SmartERP.Infrastructure.Users.Repositories;

Then register (before builder.Build()):

builder.Services.AddScoped<IUserRepository, UserRepository>();

✅ Now controller can use repository via interface.

5) Add FluentValidation (simple)
Install packages in API
We’ll use FluentValidation in API for request validation.

From solution root:

dotnet add src/SmartERP.Api/SmartERP.Api.csproj package FluentValidation.AspNetCore --version 11.10.0

If version install fails, remove --version ... and install latest.
6) Create validators in Application
Create folder:

mkdir -p src/SmartERP.Application/Users/Validation

Create validator for CreateUserRequest
src/SmartERP.Application/Users/Validation/CreateUserRequestValidator.cs

using FluentValidation;

namespaceSmartERP.Application.Users.Validation;

publicclassCreateUserRequestValidator :AbstractValidator<CreateUserRequest>
{
publicCreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}

Create validator for UpdateUserRequest
src/SmartERP.Application/Users/Validation/UpdateUserRequestValidator.cs

using FluentValidation;

namespaceSmartERP.Application.Users.Validation;

publicclassUpdateUserRequestValidator :AbstractValidator<UpdateUserRequest>
{
publicUpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}

7) Register FluentValidation in API
In src/SmartERP.Api/Program.cs add:

using FluentValidation;
using FluentValidation.AspNetCore;
using SmartERP.Application.Users.Validation;

Then before builder.Build() add:

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

8) Update Controller to use DTOs + Validation + Repository
Replace UsersController.cs with:

src/SmartERP.Api/Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Users;
using SmartERP.Application.Users.Repositories;
using SmartERP.Domain.Entities;

namespaceSmartERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
publicclassUsersController :ControllerBase
{
privatereadonly IUserRepository _repo;

publicUsersController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
publicasync Task<ActionResult<List<UserDto>>> GetAll()
    {
var users =await _repo.GetAllAsync();
var dtos = users.Select(u =>new UserDto(u.Id, u.Name)).ToList();
return Ok(dtos);
    }

    [HttpGet("{id:int}")]
publicasync Task<ActionResult<UserDto>> GetById(int id)
    {
var user =await _repo.GetByIdAsync(id);
if (userisnull)return NotFound();

return Ok(new UserDto(user.Id, user.Name));
    }

    [HttpPost]
publicasync Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request)
    {
// FluentValidation runs automatically before this method due to AddFluentValidationAutoValidation()

var user =new User { Name = request.Name };
var created =await _repo.AddAsync(user);

var dto =new UserDto(created.Id, created.Name);
return CreatedAtAction(nameof(GetById),new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
publicasync Task<IActionResult>Update(int id, [FromBody] UpdateUserRequest request)
    {
var existing =await _repo.GetByIdAsync(id);
if (existingisnull)return NotFound();

var updated =new User { Id = existing.Id, Name = request.Name };
var ok =await _repo.UpdateAsync(updated);

return ok ? NoContent() : StatusCode(500);
    }

    [HttpDelete("{id:int}")]
publicasync Task<IActionResult>Delete(int id)
    {
var ok =await _repo.DeleteAsync(id);
return ok ? NoContent() : NotFound();
    }
}

9) Run and test
cd ~/Projects/SmartERP/src/SmartERP.Api
dotnet run

In Swagger test:

POST /api/users
{"name":""}

✅ should return 400 with validation errors.

POST /api/users
{"name":"Hasibul"}

✅ should create user.