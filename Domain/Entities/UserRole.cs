using System;
using SaleemCare.Api.Domain.Entities;

namespace SaleemCare.Api.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }

    public int RoleId { get; set; }

    public User? User { get; set; }
    public Role? Role { get; set; }
}