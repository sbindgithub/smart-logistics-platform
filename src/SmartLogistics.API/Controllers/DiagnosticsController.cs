using Microsoft.AspNetCore.Mvc;

namespace SmartLogistics.API.Controllers;

[ApiController]
[Route("api/diagnostics")]
public sealed class DiagnosticsController : ControllerBase
{
    [HttpGet("simulate-timeout")]
    public async Task<IActionResult> SimulateTimeout()
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        return Ok("Simulated timeout completed");
    }

    [HttpGet("simulate-error")]
    public IActionResult SimulateError()
    {
        throw new InvalidOperationException("Simulated failure");
    }

}
