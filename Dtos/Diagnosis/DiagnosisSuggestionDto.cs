namespace SaleemCare.Api.Dtos.Diagnosis;

public class ConditionSuggestionDto
{
    public int ConditionId { get; set; }
    public string Slug { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string? NameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public int? SeverityLevel { get; set; }
    public int Score { get; set; }
}

public class DiagnosisSuggestionResponse
{
    public Guid EncounterId { get; set; }
    public List<ConditionSuggestionDto> Suggestions { get; set; } = new();
}