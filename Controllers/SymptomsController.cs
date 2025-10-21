
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

    // Get /v1/symptoms/get
    [HttpGet("get")]
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

    // GET /v1/symptoms?search=صداع&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? search, [FromQuery] int page, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.Symptoms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(q => q.NameAr.Contains(search) || q.Slug.Contains(search));
        }

        var total = await query.CountAsync();

        var rows = query
            .OrderBy(q => q.NameAr)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(q => new { q.Id, q.NameAr, q.Slug, q.DescriptionAr })
            .ToListAsync();

        return Ok(new { page, pageSize, total, items = rows });

    }

    // v1/symptoms/{id}
    [HttpGet("{id:int}")]

    public async Task<IActionResult> GetById(int id)
    {
        var s = await _db.Symptoms.AsNoTracking()
            .Where(q => q.Id == id)
            .Select(q => new { q.Id, q.NameAr, q.Slug, q.DescriptionAr })
            .FirstOrDefaultAsync();

        return s is null ? NotFound(new {error = new {message = "Symptom not found." } }) : Ok(s);

    }


    // answers history of user /v1/symptoms/{id}/answers?limit=50
    [Authorize]
    [HttpGet("{id:int}/answers")]
    public async Task<IActionResult> GetAnswersHistory(int id, [FromQuery] int limit = 50)
    {
        var userId = GetUserId();

        limit = Math.Clamp(limit, 1, 200);

        if (!await _db.Symptoms.AnyAsync(q => q.Id == id))
        { return NotFound(new { error = new { message = "Symptom not found." } }); }


        var list = await (
    from answer in _db.UserSymptomAnswers.AsNoTracking()
    join question in _db.SymptomQuestions.AsNoTracking()
        on answer.SymptomQuestionId equals question.Id
    where answer.UserId == userId && answer.SymptomId == id
    orderby answer.CreatedAt descending
    select new
    {
        answer.Id,
        answer.SymptomQuestionId,
        Question = question.QuestionAr,
        answer.AnswerJson,
        answer.CreatedAt
    }
).Take(limit).ToListAsync();


        return Ok(new { answers = list, count = list.Count });
    }

    // delete answer /v1/symptoms/{id}/answers/{answerId}
    [Authorize]
    [HttpDelete("{id:int}/answers/{answerId:long}")]
    public async Task<IActionResult> DeleteAnswer(int id, long answerId)
    {
       var userId = GetUserId();

        var answers = await _db.UserSymptomAnswers.FirstOrDefaultAsync(q => q.SymptomId == id && q.Id == answerId && q.UserId == userId);

        if (answers is null)
        {
            return NotFound(new { error = new { message = "Answer not found." } });

        }

        _db.UserSymptomAnswers.Remove(answers);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Deleted!" });
    }


}