using Microsoft.AspNetCore.Mvc;

namespace Waybon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "ok", time = DateTime.UtcNow });
    }
}