using Microsoft.EntityFrameworkCore;
using SmartERP.Domain.Entities;

namespace SmartERP.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
