namespace Hamal.Web.Contracts.Citizens;

public record UpdateCitizenRequest(
    string FirstName,
    string LastName,
    int FamilyNumber,
    bool IsLonely,
    bool IsAddressWrong,
    string? NewStreetName,
    string? NewBuildingNumber,
    int? NewFlatNumber); 