using Hamal.Domain.Entities;

namespace Hamal.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
} 