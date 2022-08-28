using Microsoft.AspNetCore.Mvc;

namespace MaestroSignant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Pong");
    }
}