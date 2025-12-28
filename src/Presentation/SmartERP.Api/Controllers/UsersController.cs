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
