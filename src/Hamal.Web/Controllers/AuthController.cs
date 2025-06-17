using Hamal.Application.Common.Interfaces;
using Hamal.Infrastructure.Persistence;
using Hamal.Web.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Web.Controllers;
/// <summary>
/// 
/// </summary>
/// <param name="dbContext"></param>
/// <param name="passwordHasher"></param>
/// <param name="jwtTokenGenerator"></param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    AppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : ControllerBase
{
    /// <summary>
    /// Returns a JWT token with encrypted user's role
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns the user list</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(new LoginResponse(jwtTokenGenerator.GenerateToken(user), (int)user.Role));
    }
} 