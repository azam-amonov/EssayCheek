using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : RESTFulController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async ValueTask<ActionResult<User>> PostUserAsync(User user)
    {
        User addedUser = await _userService.AddUserAsync(user);
        return Created(addedUser);
    }

    [HttpGet] 
    public ActionResult<IQueryable<User>> GetAllUsers()
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