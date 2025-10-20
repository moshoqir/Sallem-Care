using System.Collections.Generic;
namespace SaleemCare.Api.Domain.Entities;


public class Region
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string Slug { get; set; }
    public ICollection<RegionSymptom> RegionSymptoms { get; set; } = new List<RegionSymptom>();
}

public class RegionSymptom
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public Region Region { get; set; }
    public int SymptomId { get; set; }
    public Symptom Symptom { get; set; }
}