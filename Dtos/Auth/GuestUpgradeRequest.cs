using System;

namespace SaleemCare.Api.Dtos.Auth;

public class GuestUpgradeRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FullName { get; set; }
    
}
