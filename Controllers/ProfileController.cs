using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Domain.Entities;
using SaleemCare.Api.Dtos.Profile;
using SaleemCare.Api.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/profile")]
[Authorize]


public class ProfileController: ControllerBase

{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProfileController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]

    public async Task<IActionResult> Get()
    {
        var userId = User.GetUserId();
        var profile = await _db.UserProfiles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (profile is null) return Ok(new { profile = (object?)null });

        // return list surgical history

        var history = string.IsNullOrWhiteSpace(profile.SurgicalHistoryJson)
                ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(profile.SurgicalHistoryJson);


        return Ok(new
        {
            profile = new
            {
                profile.Id,
                profile.UserId,
                profile.Gender,
                profile.HasDiabetes,
                profile.HasHypertension,
                profile.IsAthlete,
                SurgicalHistory = history,
                profile.Dob,
                profile.CreatedAt
            }
        });
    }

    [HttpPost("questionnaire")]
    public async Task<IActionResult> save([FromBody] SaveProfileDto dto)
    {
        var user = User.GetUserId();

        var entity = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user);

        if (entity is null)
        {
            entity = _mapper.Map<UserProfile>(dto);
            entity.UserId = user;

            _db.UserProfiles.Add(entity);
        }
        else
        {
            _mapper.Map(dto,entity);
        }

        await _db.SaveChangesAsync();

        return StatusCode(201, new { message = "Saved", profileId = entity.Id });
    }
}