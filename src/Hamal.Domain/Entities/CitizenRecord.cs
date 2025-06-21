using Hamal.Domain.Enums;

namespace Hamal.Domain.Entities;

public class CitizenRecord
{
    public int Id { get; set; }
    public int Fid { get; set; }
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
    public CitizenStatus StatusInCallCenter { get; set; }
    public string? Phone1 { get; set; } = string.Empty;
    public string? Phone2 { get; set; } = string.Empty;
    public string? Phone3 { get; set; } = string.Empty;
    public bool IsAnsweredTheCall { get; set; }
    public bool HasMamad { get; set; }
    public bool HasMiklatPrati { get; set; }
    public bool HasMiklatZiburi { get; set; }
    public bool HasMobilityRestriction { get; set; }
    public bool IsDead { get; set; }
    public bool HasTemporaryAddress { get; set; }
    public string? TemporaryStreetName { get; set; }
    public string? TemporaryBuildingNumber { get; set; }
    public string? TemporaryFlat { get; set; }
    public Guid? LockedByUserId { get; set; }
    public DateTime? LockedUntil { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? LastUpdatedByUserId { get; set; }
    
}