namespace Hamal.Application.Citizens.Commands;

public record UpdateCitizenCommand(
    int Id,
    string FirstName,
    string LastName,
    int FamilyNumber,
    bool IsLonely,
    bool IsAddressWrong,
    string? NewStreetName,
    string? NewBuildingNumber,
    int? NewFlatNumber
); 