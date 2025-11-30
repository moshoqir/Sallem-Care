using SaleemCare.Api.Services.Excel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleemCare.Api.Extensions;
using System.IO;
namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/admin/excel")]
[Authorize(Roles= "Admin")]

public class AdminExcelController : ControllerBase
{
    private readonly ExcelImportService _excel;

    public AdminExcelController(ExcelImportService excel)
    {
        _excel = excel;
    }

    /// <summary>
    /// Uploads an Excel file and returns a preview of Conditions and SymptomConditionMap
    /// without saving anything to the database.
    /// </summary>
    /// 

    [HttpPost("upload")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
    public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new {error = "No file uploaded." });
        }

        // get file extension
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (ext != ".xlsx")
        {
            return BadRequest(new { error = "Only .xlsx files are supported." });
        }

        var userId = User.GetUserId();

        // read file stram
        await using var stream = file.OpenReadStream();
        // process preview
        var preview = await _excel.PreviewAsync(stream, file.FileName, userId, ct);

        return Ok(new
        {
            preview.FileName,
            Errors = preview.Errors,
            ConditionsCount = preview.Conditions.Count,
            SymptomConditionCount = preview.SymptomConditions.Count,
            Conditions = preview.Conditions.Take(20),
            SymptomConditions = preview.SymptomConditions.Take(20)
        });

    }



    /// <summary>
    /// Uploads an Excel file and IMPORTS its contents into the database
    /// (Conditions + SymptomConditionMap). Returns summary + errors.
    /// </summary>
    /// 
    [HttpPost("commit")]
    [RequestSizeLimit(10*1024*1024)]
    public async Task<IActionResult> Commit([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "No file uploaded." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext != ".xlsx")
        {
            return BadRequest(new { error = "Only .xlsx files are supported." });
        }

        var userId = User.GetUserId();

        await using var stream = file.OpenReadStream();

        var result = await _excel.ImportAsync(stream, file.FileName, userId, ct);

        return Ok(new
        {
            result.FileName,
            result.Errors,
            result.ConditionsInserted,
            result.ConditionsUpdated,
            result.LinksInserted,
            result.LinksUpdated,
            result.TotalRowsImported
        });
    }
}