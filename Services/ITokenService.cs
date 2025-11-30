using SaleemCare.Api.Domain.Entities;
using System;

public interface ITokenService
{
    string Create(User user, IEnumerable<string> roles, TimeSpan? lifetime = null);
}