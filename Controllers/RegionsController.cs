

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/regions")]

public class RegionsController : ControllerBase
{
    private readonly AppDbContext _db;
    public RegionsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetRegions()
        => Ok(await _db.Regions.AsNoTracking()
            .OrderBy(r => r.Id)
            .Select(r => new { r.Id, r.NameAr, r.Slug })
            .ToListAsync());

    [HttpGet("{id:int}/symptoms")]
    public async Task<IActionResult> GetRegionSymptoms(int id)
    {
        var regionExists = await _db.Regions.AnyAsync(r => r.Id == id);
        if (!regionExists) return NotFound(new { error = new { message = "Region not found." } });

        var symptoms = await _db.RegionSymptoms
            .Where(m => m.RegionId == id)
            .Select(m => new { m.Symptom.Id, m.Symptom.NameAr, m.Symptom.Slug })
            .OrderBy(s => s.Id)
            .ToListAsync();

        return Ok(symptoms);
    }
}