using System;

namespace SaleemCare.Api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; //unique
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsGuest { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? GuestExpiresAt { get; set; }

}