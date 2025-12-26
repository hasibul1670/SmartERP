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
