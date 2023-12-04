using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync(User user)
    {
        await _userService.AddUserAsync(user);
        return Ok(user);
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAsync()
    {
        var data = await _userService.RetrieveAllUsers().ToListAsync();
        return Ok(data);
    }
}