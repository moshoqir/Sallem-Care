using AutoMapper;
using SaleemCare.Api.Domain.Entities;
using SaleemCare.Api.Dtos.Profile;

namespace SaleemCare.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SaveProfileDto, UserProfile>()
      .ForMember(d => d.SurgicalHistoryJson,
          o => o.MapFrom(s => s.SurgicalHistory == null
              ? null
              : System.Text.Json.JsonSerializer.Serialize(s.SurgicalHistory, (System.Text.Json.JsonSerializerOptions)null)
          )
      );
    }
}