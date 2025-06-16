using System.Security.Claims;

namespace Hamal.Web.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User ID not found in token.");
        return Guid.Parse(userId);
    }
} 