using SaleemCare.Api.Domain.Entities;
using System;
using System.Collections.Generic;

public interface ITokenService
{
    string Create(User user, IEnumerable<string> roles, TimeSpan? lifetime = null);
}