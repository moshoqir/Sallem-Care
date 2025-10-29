
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Domain.Entities;
using SaleemCare.Api.Extensions;

namespace SaleemCare.Api.Controllers;


[ApiController]
[Route("v1/encounters")]
[Authorize]

public class EncountersController : ControllerBase
{
    private readonly AppDbContext _db;
    public EncountersController(AppDbContext db) => _db = db;



    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? status, [FromQuery] int limit = 20)
    {
        var UserId = User.GetUserId();

        var q = _db.Encounters.Where(u => u.UserId == UserId);
        if (status == "open") q = q.Where(x => x.EndedAt == null);
        if (status == "closed") q = q.Where(x => x.EndedAt != null);

        var data = await q.OrderByDescending(x => x.StartedAt)
            .Take(limit)
            .Select(d => new { d.Id, d.StartedAt, d.EndedAt, d.Note })
            .ToListAsync();

        return Ok(new { data });
    }


    public record StartDto(string? Note);

    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartDto? dto)
    {
        var userId = User.GetUserId();

        var encounter = new Encounter { UserId = userId, Note = dto?.Note };

        _db.Encounters.Add(encounter);
        await _db.SaveChangesAsync();

        return StatusCode(201, new { encounterId = encounter.Id, encounter.StartedAt });
    }

    [HttpPatch("{id:guid}/end")]
    public async Task<IActionResult> End(Guid id)
    {
        var userId = User.GetUserId();

        var e = await _db.Encounters.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (e is null) return NotFound();

        if (e.EndedAt != null) return BadRequest(new { error = new { message = "Session already ended." } });

        e.EndedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new { message = "Ended", e.EndedAt });

    }

    [HttpGet("{id:guid}/answers")]
    public async Task<IActionResult> Answers(Guid id)
    {
        var userId = User.GetUserId();

        var owned = await _db.Encounters.AnyAsync(x => x.Id == id && x.UserId == userId);

        if (!owned) return NotFound();

        var data = await _db.UserSymptomAnswers
            .Where(a => a.UserId == userId && a.EncounterId == id)
            .OrderBy(a => a.CreatedAt)
            .Select(q => new { q.Id, q.SymptomId, q.SymptomQuestionId, q.AnswerJson, q.CreatedAt })
            .ToListAsync();


        return Ok(new { data });
    }

}