using System.Collections;
using System.Collections.Generic;
namespace SaleemCare.Api.Domain.Entities;


public class Symptom
{
    public int Id { get; set; }
    public string NameAr { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? DescriptionAr { get; set; }
    public ICollection<SymptomQuestion> Questions { get; set; } = new List<SymptomQuestion>();

}