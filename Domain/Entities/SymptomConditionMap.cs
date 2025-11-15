namespace SaleemCare.Api.Domain.Entities;

public class SymptomConditionMap
{
    public int Id { get; set; }

    public int SymptomId { get; set; }
    public Symptom? Symptom { get; set; }

    public int ConditionId { get; set; }
    public Condition? Condition { get; set; }

    public int Relevance { get; set; }
}