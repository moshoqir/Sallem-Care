namespace SaleemCare.Api.Domain.Entities;

public class SymptomQuestion
{
    public int Id { get; set; }
    public int SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;
    public string QuestionAr { get; set; } = null!;
    public string Type { get; set; } = "single"; // single, multiple, text, number
    public string? OptionsArJson { get; set; } // JSON array of options for single and multiple types
    public byte Order { get; set; }
}