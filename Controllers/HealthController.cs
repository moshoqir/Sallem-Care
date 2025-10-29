using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("")]

public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Root() => Ok(new { name = "SaleemCare.Api", status = "ok" });

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy", time = DateTime.UtcNow });
}