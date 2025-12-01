using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Dtos.Diagnosis;
using SaleemCare.Api.Extensions;
namespace SaleemCare.Api.Controllers;


[ApiController]
[Route("v1/encounters")]
[Authorize]

public class DiagnosisController : ControllerBase
{
    private readonly AppDbContext _db;

    public DiagnosisController(AppDbContext db)
    {
        _db = db;
    }

    /* 
    Returns Top 3 suggested conditions for the currenct encounter (session)
    ==> based on the given symptoms that have answers in that encounter
     */
    [HttpGet("{encounterId:Guid}/diagnosis")]
    public async Task<IActionResult> Suggest(Guid encounterId, CancellationToken ct)
    {
        var userId = User.GetUserId();

        // 1. check if this encounter belongs to this user
        var encounterOwned = await _db.Encounters
            .AnyAsync(e => e.Id == encounterId && e.UserId == userId);

        if (!encounterOwned) return NotFound(new { error = "Encounter Not Found." });

        // 2. get the IDs of the symptoms that have answers in this session
        var symptomIds = await _db.UserSymptomAnswers
            .Where(a => a.UserId == userId && a.EncounterId == encounterId)
            .Select(a => a.SymptomId)
            .Distinct()
            .ToListAsync(ct);

        if (symptomIds.Count == 0)
        {
            return Ok(new DiagnosisSuggestionResponse
            {
                EncounterId = encounterId,
                Suggestions = new List<ConditionSuggestionDto>()
            });
        }

        // 3. Join with SymptomConditionMap + Conditions to compute scores
        var query =
            from map in _db.SymptomConditionMap
            join condition in _db.Conditions on map.ConditionId equals condition.Id
            where symptomIds.Contains(map.SymptomId)
            select new
            {
                condition.Id,
                condition.Slug,
                condition.NameAr,
                condition.NameEn,
                condition.DescriptionAr,
                condition.DescriptionEn,
                condition.SeverityLevel,
                map.Relevance,

            };

        var grouped = await query
            .GroupBy(x => new
            {
                x.Id,
                x.Slug,
                x.NameAr,
                x.NameEn,
                x.DescriptionAr,
                x.DescriptionEn,
                x.SeverityLevel
            })
            .Select(g => new ConditionSuggestionDto
            {
                ConditionId = g.Key.Id,
                Slug = g.Key.Slug,
                NameAr = g.Key.NameAr,
                NameEn = g.Key.NameEn,
                DescriptionAr = g.Key.DescriptionAr,
                DescriptionEn = g.Key.DescriptionEn,
                SeverityLevel = g.Key.SeverityLevel,

                // Score = sum of relevanvce + severity 
                Score = g.Sum(x => x.Relevance) + (g.Key.SeverityLevel ?? 0)

            })
            .OrderByDescending(c => c.Score)
            .Take(3)
            .ToListAsync(ct);

        var response = new DiagnosisSuggestionResponse
        {
            EncounterId = encounterId,
            Suggestions = grouped
        };

        return Ok(response);

    }
}
