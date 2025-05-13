using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController(ILogger<ProductsController> logger, WeatherHttpClient httpClient)
    : ControllerBase
{

    private readonly ILogger<ProductsController> _logger = logger;

    [HttpGet(Name = "GetWeather")]
    public async Task<IEnumerable<WeatherForecast>?> Get()
    {
        return await httpClient.GetWeatherForecast();
    }
}