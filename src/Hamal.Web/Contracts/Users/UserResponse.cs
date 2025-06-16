using Hamal.Domain.Enums;

namespace Hamal.Web.Contracts.Users;

public record UserResponse(Guid Id, string Username, Role Role); 