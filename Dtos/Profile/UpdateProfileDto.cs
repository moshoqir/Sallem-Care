

using System.Collections.Generic;

namespace SaleemCare.Api.Dtos.Profile;

public class UpdateProfileDto()
{
    public string? Gender { get; set; }
    public bool? HasDiabetes { get; set; }
    public bool? HasHypertension { get; set; }
    public bool? IsAthlete { get; set; }
    public List<string>? SurgicalHistory { get; set; }
    public DateOnly? Dob { get; set; }

}