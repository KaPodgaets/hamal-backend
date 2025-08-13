using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;
using Hamal.Web.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = nameof(Role.Admin))]
[Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.ShiftManager)}")]
public class UsersController(AppDbContext dbContext, IPasswordHasher passwordHasher) : ControllerBase
{
    /// <summary>
    /// Returns a list of users
    /// </summary>
    /// <response code="200">Returns the user list</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(List<UserResponse>), 200)]
    [ProducesResponseType(401)]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await dbContext.Users
            .Select(u => new UserResponse(u.Id, u.Username, u.Role))
            .ToListAsync();
        return Ok(users);
    }

    /// <summary>
    /// Returns a user
    /// </summary>
    /// <response code="200">Returns a user</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(401)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(new UserResponse(user.Id, user.Username, user.Role));
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <response code="200">Returns a CreatedAtActionResult</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Password is required.");
        }

        if (await dbContext.Users.AnyAsync(u => u.Username == request.Username))
        {
            return Conflict("User with this username already exists.");
        }

        var user = Domain.Entities.User.Create(
            request.Username,
            passwordHasher.HashPassword(request.Password),
            request.Role);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var response = new UserResponse(user.Id, user.Username, user.Role);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
    }

    /// <summary>
    /// Changes user's password
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] string newPassword)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null) return NotFound();

        if (string.IsNullOrEmpty(newPassword))
        {
            return BadRequest("Password is required.");
        }

        var passwordHash = passwordHasher.HashPassword(newPassword);
        user.ChangePassword(passwordHash);

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <response code="200">No Content Result</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null) return NotFound();

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Sets the IsDisabled property for a user
    /// </summary>
    /// <param name="id">User Id</param>
    /// <param name="newDisableStatus">New value for IsDisabled</param>
    /// <response code="200">No Content Result</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">User not found</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpPut("{id:guid}/disable-status")]
    public async Task<IActionResult> SetNewDisableStatus(Guid id, [FromBody] bool newDisableStatus)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null) return NotFound();
        
        if (newDisableStatus)
        {
            user.Disable();
        }
        else
        {
            user.Enable();
        }

        await dbContext.SaveChangesAsync();
        return NoContent();
    }
    
    /// <summary>
    /// Sets the IsDisabled = true for all users with the role "Operator"
    /// </summary>
    /// <param name="newDisableStatus">New value for IsDisabled</param>
    /// <response code="200">No Content Result</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">User not found</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpPut("disable-status")]
    public async Task<IActionResult> SetNewDisableStatusForEveryOne([FromBody] bool newDisableStatus)
    {
        var users = dbContext.Users
            .Where(x => x.Role == Role.Operator)
            .ToList();
        
        if (users.Count == 0) return NoContent();
        
        if (newDisableStatus)
        {
            foreach (var user in users)
            {
                user.Enable();
            }
        }
        else
        {
            foreach (var user in users)
            {
                user.Enable();
            }
        }

        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}