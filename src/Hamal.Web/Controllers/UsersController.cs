using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;
using Hamal.Web.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Role.Admin))]
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
    /// Create new user
    /// </summary>
    /// <response code="200">Returns a CreatedAtActionResult</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (await dbContext.Users.AnyAsync(u => u.Username == request.Username))
        {
            return Conflict("User with this username already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            Role = request.Role
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var response = new UserResponse(user.Id, user.Username, user.Role);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <response code="200">Returns a CreatedAtActionResult</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CreateUserRequest request)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null) return NotFound();

        if (user.Username != request.Username && await dbContext.Users.AnyAsync(u => u.Username == request.Username))
        {
            return Conflict("User with this username already exists.");
        }

        user.Username = request.Username;
        user.Role = request.Role;
        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = passwordHasher.HashPassword(request.Password);
        }

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
} 