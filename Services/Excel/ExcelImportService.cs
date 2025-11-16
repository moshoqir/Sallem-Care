using System.Text.Json;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Threading;



namespace SaleemCare.Api.Services.Excel;

public class ExcelPreviewResult
{
    public string FileName { get; set; } = null!;
    public List<string> Errors { get; set; } = new();
    public List<ConditionPreview> Conditions { get; set; } = new();
    public List<SymptomConditionPreview> SymptomConditions { get; set; } = new();
}

public class ConditionPreview
{
    public string Slug { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string? NameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public int? SeverityLevel { get; set; }
}

public class SymptomConditionPreview
{
    public string SymptomSlug { get; set; } = null!;
    public string ConditionSlug { get; set; } = null!;
    public int Relevance { get; set; }
}

/// <summary>
/// Reads Excel files and returns a preview of Conditions + SymptomCondition mappings.
/// Expected structure:
///   Sheet "Conditions":
///     A: Slug (required)
///     B: NameAr (required)
///     C: NameEn (optional)
///     D: DescriptionAr (optional)
///     E: DescriptionEn (optional)
///     F: SeverityLevel (optional int)
///
///   Sheet "SymptomConditionMap":
///     A: SymptomSlug (required)
///     B: ConditionSlug (required)
///     C: Relevance (optional int, default 5)
/// </summary>
/// 

public class ExcelImportService
{
    public async Task<ExcelPreviewResult> PreviewAsync(Stream fileStream, string fileName, Guid importedBy, CancellationToken ct = default)
    {
        // importedBy will, for now, be used for commit and history

        var result = new ExcelPreviewResult
        {
            FileName = fileName
        };

        // ClosedXML needs a seekable stream (to find one)
        MemoryStream ms = new();
        await fileStream.CopyToAsync(ms, ct);
        ms.Position = 0;

        using var wb = new XLWorkbook(ms);


        // parse conditions sheet...
        if (!wb.TryGetWorksheet("Conditions", out var wsConditions))
        {
            result.Errors.Add("Missing sheet 'Conditions'.");
        }

        else
        {
            ParseConditionsSheet(wsConditions, result);
        }

        // parse sympotmConditionMap sheet...
        if (!wb.TryGetWorksheet("SymptomConditionMap", out var wsSymptomConditionMap))
        {
            result.Errors.Add("Missing sheet 'SymptomConditionMap'.");
        }
        else
        {
            ParseSymptomConditionMapSheet(wsSymptomConditionMap, result);

        }

        return result;
    }

    private static void ParseConditionsSheet(IXLWorksheet ws, ExcelPreviewResult result)
    {
        var row = 2; // staert after header

        while (true)
        {
            var slug = ws.Cell(row, 1).GetString().Trim();
            var nameAr = ws.Cell(row, 2).GetString().Trim();

            if (string.IsNullOrWhiteSpace(slug) && string.IsNullOrWhiteSpace(nameAr))
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(slug))
            {
                result.Errors.Add($"Conditions: Row {row} : Missing Slug.");
                row++;
                continue;
            }

            if (string.IsNullOrWhiteSpace(nameAr))
            {
                result.Errors.Add($"Conditions: Row {row} : Missing NameAr.");
                row++;
                continue;
            }

            int? severity = null;
            var sevStr = ws.Cell(row, 6).GetString().Trim();
            if (!string.IsNullOrWhiteSpace(sevStr) && int.TryParse(sevStr, out var sevVal))
            {
                severity = sevVal;
            }

            result.Conditions.Add(new ConditionPreview
            {
                Slug = slug,
                NameAr = nameAr,
                NameEn = ws.Cell(row, 3).GetString().Trim(),
                DescriptionAr = ws.Cell(row, 4).GetString().Trim(),
                DescriptionEn = ws.Cell(row, 5).GetString().Trim(),
                SeverityLevel = severity
            });
            row++;
        }
    }

    private static void ParseSymptomConditionMapSheet(IXLWorksheet ws, ExcelPreviewResult result)
    {
        var row = 2;

        while (true)
        {
            var symptomSlug = ws.Cell(row, 1).GetString().Trim();
            var conditionSlug = ws.Cell(row, 2).GetString().Trim();

            if (string.IsNullOrWhiteSpace(symptomSlug) && string.IsNullOrWhiteSpace(conditionSlug))
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(symptomSlug) || string.IsNullOrWhiteSpace(conditionSlug))
            {
                result.Errors.Add($"SymptomConditionMap: Row {row} : Missing SymptomSlug or conditionSlug.");
                row++;
                continue;
            }

            var relStr = ws.Cell(row, 3).GetString().Trim();
            var relevance = 5;
            if (!string.IsNullOrWhiteSpace(relStr) && int.TryParse(relStr, out var relVal))
            {
                relevance = relVal;
            }

            result.SymptomConditions.Add(new SymptomConditionPreview
            {
                SymptomSlug = symptomSlug,
                ConditionSlug = conditionSlug,
                Relevance = relevance
            });

            row++;

        }
    }
}
