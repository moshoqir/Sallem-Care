using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Domain.Entities;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SaleemCare.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly AppDbContext _db;
    public MeController(AppDbContext db) => _db = db;


    [Authorize]
    [HttpGet]

    public async Task<IActionResult> Me()
    {

        var userId = User.GetUserId();
        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, u.Name, u.Email, u.CreatedAt,
                Profile =
                 _db.UserProfiles
                .Where(q => q.UserId == userId)
                .Select(p => new {
                    p.Gender,
                    p.HasDiabetes,
                    p.HasHypertension,
                    p.IsAthlete,
                    p.SurgicalHistoryJson,
                    p.Dob
                })
            .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (user is null) return NotFound(new { error = new { message = "User not found." } });
        return Ok(user);

    }

    [Authorize]
    [HttpGet("answers")]

    public async Task<IActionResult> getAnswers([FromQuery] int limit = 50, [FromQuery] DateTime? since = null)
    {
        var userId = User.GetUserId();
        limit = Math.Clamp(limit, 1, 200);

        var query = _db.UserSymptomAnswers
            .Where(a => a.UserId == userId);

        if (since.HasValue) { query = query.Where(a => a.CreatedAt >= since.Value); }

        var result = await query
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .Select(a => new
            {
                a.Id,
                a.SymptomId,
                SymptomName = a.SymptomId != 0 ? _db.Symptoms.Where(s => s.Id == a.SymptomId).Select(d => new { d.NameAr }).FirstOrDefault() : null,
                a.SymptomQuestionId,
                SymptomQuestion = a.SymptomQuestionId != 0 ? _db.SymptomQuestions.Where(d => d.Id == a.SymptomQuestionId).Select(q => q.QuestionAr).FirstOrDefault() : null,
                a.AnswerJson,
                a.CreatedAt
            }).ToListAsync();

        return Ok(new { count = result.Count, result });


    }

    [HttpGet("timeline")]
    public async Task<IActionResult> Timeline([FromQuery] int days = 14)
    {
        var userId = User.GetUserId();
        var since = DateTime.UtcNow.Date.AddDays(-Math.Max(1, days));

        var data = await _db.UserSymptomAnswers
                    .Where(q => q.UserId == userId && q.CreatedAt >= since)
                    .GroupBy(a => a.CreatedAt.Date)
                    .Select(q => new { day = q.Key, count = q.Count(), last = q.Max(x => x.CreatedAt) })

                    .OrderBy(a => a.day)
                    .ToListAsync();

        return Ok(new { since, days, data = data });
    }


}