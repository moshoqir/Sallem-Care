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
}