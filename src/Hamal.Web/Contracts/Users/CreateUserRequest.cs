using Hamal.Domain.Enums;

namespace Hamal.Web.Contracts.Users;

public record CreateUserRequest(string Username, string Password, Role Role); 