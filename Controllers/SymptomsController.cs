
using SaleemCare.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;


namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/symptoms")]

public class SymptomsController : ControllerBase
{
    private readonly AppDbContext _db;
    public SymptomsController(AppDbContext db) => _db = db;


    private Guid GetUserId()
    {
        var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(sub!);
    }

    // Get /v1/symptoms
    [HttpGet]
    public async Task<IActionResult> GetSymptoms()
        => Ok(await _db.Symptoms.AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new { s.Id, s.NameAr, s.Slug, s.DescriptionAr })
            .ToListAsync());


    // GET /v1/symptoms/{id}/questions
    [HttpGet("{id:int}/questions")]

    public async Task<IActionResult> GetQuestions(int id)
    {
        var symptom = await _db.Symptoms.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (symptom is null) return NotFound(new { error = new { message = "symptom not found." } });

        var questions = await _db.SymptomQuestions
            .Where(q => q.SymptomId == id)
            .OrderBy(q => q.Id)
            .Select(q => new
            {
                q.Id,
                q.QuestionAr,
                q.Type,
                OptionsAr = q.OptionsArJson
            }).ToListAsync();

        return Ok(new { symptom = new { symptom.Id, symptom.NameAr }, questions = questions });

    }

    // POST /v1/symptoms/{id}/answers
    public record AnswerQuestion(int QuestionId, string Answer);
    public record SaveAnswersDto(List<AnswerQuestion> Answers);

    [Authorize]
    [HttpPost("{id:int}/answers")]
    public async Task<IActionResult> SaveAnswers(int id, [FromBody] SaveAnswersDto dto)
    {
        // validate symptom exists
        if (!await _db.Symptoms.AnyAsync(s => s.Id == id))
        {
            return NotFound(new { error = new { message = "symptom not found." } });
        }

        //// temp UserId
        var userId = GetUserId();

        // store answers (for each since it'll be several)

        foreach (var answer in dto.Answers)
            {
            var valid = await _db.SymptomQuestions.AnyAsync(q => q.Id == answer.QuestionId && q.SymptomId == id);
            if (!valid)
            {
                return BadRequest(new { error = new { message = $"Invalid question: {answer.QuestionId}" } });
            }

            // conver answers to json
            var json = System.Text.Json.JsonSerializer.Serialize(answer.Answer);

            //check if user exists

            var existing = await _db.UserSymptomAnswers
                .FirstOrDefaultAsync(x => x.UserId == userId && x.SymptomId == id && x.SymptomQuestionId == answer.QuestionId);

            if (existing is null)
            {
                _db.UserSymptomAnswers.Add(new() 
                {

                    UserId = userId,
                    SymptomId = id,
                    SymptomQuestionId = answer.QuestionId,
                    AnswerJson = json,

                });
            }
            else
            {
                existing.AnswerJson = json;
            }
        }

        await _db.SaveChangesAsync();
        return StatusCode(201, new { message = "Answers saved." });
    }
}