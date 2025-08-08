using weather.Application.Contracts;
using weather.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace weather.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpPost("detail")]
    public async Task<ActionResult<WeatherResponse>> GetByCityIdAsync([FromBody] WeatherRequest request, CancellationToken cancellationToken)
    {
        var result = await _weatherService.GetWeatherByCityIdAsync(request.CityId, cancellationToken);
        return Ok(result);
    }
}