using System;

namespace SaleemCare.Api.Domain.Entities;

public class ExcelImport
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public Guid ImportedBy { get; set; }
    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
    public int RowsImported { get; set; }
}