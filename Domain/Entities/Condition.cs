
namespace SaleemCare.Api.Domain.Entities;

public class Condition
{
    public int Id { get; set; }
    public string NameAr { get; set; } = null!;
    public string? NameEn {get; set;}
    public string Slug { get; set; } = null!;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public int? SeverityLevel { get; set; }

}