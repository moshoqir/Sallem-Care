namespace SaleemCare.Api.Domain.Entities;

using System;

public class UserProfile
{
    public int Id { get; set; }
    public Guid userId { get; set; }
    public string? Gender { get; set; }
    public bool HasDiabetes { get; set; }
    public bool HasHypertension { get; set; }
    public bool IsAthlete { get; set; }
    public string? SurgicalHistoryJson { get; set; }
    public DateOnly? Dob { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}