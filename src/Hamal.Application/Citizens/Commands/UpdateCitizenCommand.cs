namespace Hamal.Application.Citizens.Commands;

public record UpdateCitizenCommand(
    int Id,
    string StreetName,
    string BuildingNumber,
    string FlatNumber,
    string FirstName,
    string LastName,
    int FamilyNumber,
    bool IsLonely,
    bool IsAddressWrong,
    string? NewStreetName,
    string? NewBuildingNumber,
    string? NewFlatNumber,
    string? Phone1,
    string? Phone2,
    string? Phone3,
    bool IsAnsweredTheCall
); 