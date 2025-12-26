using SmartERP.Domain.Entities;

namespace SmartERP.Application.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetAll();
}
