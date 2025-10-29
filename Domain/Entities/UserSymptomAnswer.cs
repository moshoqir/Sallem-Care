using System;
namespace SaleemCare.Api.Domain.Entities;


public class UserSymptomAnswer
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int SymptomId { get; set; }
    public int SymptomQuestionId { get; set; }
    public string AnswerJson { get; set; } = null!;
    public Guid? EncounterId { get; set; }
    public Encounter? Encounter { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}