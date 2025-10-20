using SaleemCare.Api.Domain.Entities;
using System;

public interface ITokenService
{
    string Create(User user, TimeSpan? lifetime = null);
}