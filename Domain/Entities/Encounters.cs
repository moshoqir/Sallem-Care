using System;
using Microsoft.EntityFrameworkCore;
namespace SaleemCare.Api.Domain.Entities;



public class Encounter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
    public string? Note { get; set; }
}

