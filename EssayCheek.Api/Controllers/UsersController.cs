using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using EssayCheek.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController : RESTFulController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) =>
        _userService = userService;

    [HttpPost("create-user")]
    public async ValueTask<ActionResult<User>> PostUserAsync(User user)
    {
        try
        {
            User addedUser = await _userService.AddUserAsync(user);
            return Created(addedUser);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
                        when (userDependencyValidationException.InnerException is AlreadyExistsUserException)
        {
            return Conflict(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
    }

    [HttpGet("get-all-users")]
    public ActionResult<IQueryable<User>> GetAllUsers()
    {
        try
        {
            IQueryable<User> retrievedUsers = _userService.RetrieveAllUsers();
            return Ok(retrievedUsers);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException);
        }
    }

    [HttpGet("get-user-by-id/{userId}")]
    public async ValueTask<ActionResult<User>> GetUserByIdAsync(Guid userId)
    {
        try
        {
            User user = await _userService.RetrieveUserByIdAsync(userId);
            return Ok(user);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException);
        }
    }

    [HttpPut("update-user")]
    public async ValueTask<ActionResult<User>> PutUserAsync(User user)
    {
        try
        {
            User modifiedUser = await _userService.ModifyUserAsync(user);
            return Ok(modifiedUser);
        }
        catch (UserValidationException userValidationException)
                        when (userValidationException.InnerException is NotFoundUserException)
        {
            return NotFound(userValidationException.InnerException);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
                        when (userDependencyValidationException.InnerException is AlreadyExistsUserException)
        {
            return Conflict(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException.InnerException);
        }
    }

    [HttpDelete("delete-user-by-id/{userId}")]
    public async ValueTask<ActionResult<User>> DeleteUserAsync(Guid userId)
    {
        try
        {
            User user = await _userService.RemoveUserByIdAsync(userId);
            return Ok(user);
        }
        catch (UserValidationException userValidationException)
                        when (userValidationException.InnerException is NotFoundUserException)
        {
            return NotFound(userValidationException.InnerException);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
                        when (userDependencyValidationException.InnerException is LockedUserException)
        {
            return Locked(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
        {
            return BadRequest(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException.InnerException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException.InnerException);
        }
    }
}