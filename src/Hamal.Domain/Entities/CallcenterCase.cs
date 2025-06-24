namespace Hamal.Domain.Entities;

public class CallcenterCase
{
    public int Id { get; init; }
    public string CallcenterCaseNumber { get; init; } = string.Empty;
    public int CitizenRecordId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    
    // Navigation property
    public CitizenRecord CitizenRecord { get; init; } = null!;
} 