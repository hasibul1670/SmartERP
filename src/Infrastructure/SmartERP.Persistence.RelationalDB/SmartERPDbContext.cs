using System;
using SmartERP.Domain.Entities;

namespace SmartERP.Infrastructure.Persistence.RelationalDB;

public class SmartERPDbContext
{
    public SmartERPDbContext(DbContextOptions<SmartERPDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}

public class DbContextOptions<T>
{
}