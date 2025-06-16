using Hamal.Domain.Enums;

namespace Hamal.Domain.Entities;

public class Citizen
{
    public int Id { get; set; }
    public string StreetName { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string FlatNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int FamilyNumber { get; set; }
    public bool IsLonely { get; set; }
    public bool IsAddressWrong { get; set; }
    public string? NewStreetName { get; set; }
    public string? NewBuildingNumber { get; set; }
    public string? NewFlatNumber { get; set; }
    public CitizenStatus Status { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? LockedUntil { get; set; }
    public DateTime LastUpdatedAt { get; set; }
} 