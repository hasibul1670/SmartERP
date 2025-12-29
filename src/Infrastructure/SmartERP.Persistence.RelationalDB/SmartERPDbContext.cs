using Microsoft.EntityFrameworkCore;
using SmartERP.Domain.Entities;

//test
namespace SmartERP.Persistence.RelationalDB;

public class SmartERPDbContext(DbContextOptions<SmartERPDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}