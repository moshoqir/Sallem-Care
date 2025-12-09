namespace SaleemCare.Api.Domain.Entities;

public class ConditionRule
{
    public int Id { get; set; }
    public int ConditionId { get; set; }
    public Condition? Condition { get; set; }

    public string RuleText { get; set; } = null!;
    public int ScoreBonus { get; set; }
}