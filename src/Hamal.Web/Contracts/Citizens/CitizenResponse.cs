using Hamal.Domain.Enums;

namespace Hamal.Web.Contracts.Citizens;

public record CitizenResponse(
    int Id,
    string StreetName,
    string BuildingNumber,
    string FlatNumber,
    string FirstName,
    string LastName,
    int FamilyNumber,
    bool IsLonely,
    bool IsAddressWrong,
    CitizenStatus Status,
    DateTime? LockedUntil,
    DateTime LastUpdatedAt
); 