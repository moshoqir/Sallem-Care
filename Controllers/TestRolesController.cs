using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Domain.Entities;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SaleemCare.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SaleemCare.Api.Controllers;

[Authorize(Roles = "Clinician")]
[ApiController]
[Route("v1/ping")]
public class AdminPingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping() => Ok(new { ok = true, role = "Clinician" });
}

