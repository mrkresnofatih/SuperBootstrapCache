using DesertCamel.BaseMicroservices.SuperBootstrap.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Sample.ProjectOne.WebApi.Controllers;

[ApiController]
[Route("weather")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISuperBootstrapCache _superBootstrapCache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        ISuperBootstrapCache superBootstrapCache)
    {
        _logger = logger;
        _superBootstrapCache = superBootstrapCache;
    }

    [HttpGet("set/{city}/{temperature}")]
    public async Task<object> Set([FromRoute] string city, [FromRoute] int temperature)
    {
        var storeResult = await _superBootstrapCache.Store(new StoreRequest
        {
            Key = city,
            Value = new WeatherReport
            {
                City = city,
                Temperature = temperature
            },
            TimeToLive = 30
        });
        return storeResult;
    }

    [HttpGet("get/{city}")]
    public async Task<object> Get([FromRoute] string city)
    {
        var getResult = await _superBootstrapCache.Fetch<WeatherReport>(new FetchRequest
        {
            Key = city
        });
        return getResult;
    }

    [HttpGet("delete/{city}")]
    public async Task<object> Delete([FromRoute] string city)
    {
        var deleteResult = await _superBootstrapCache.Delete(new DeleteRequest
        {
            Key = city,
        });
        return deleteResult;
    }

    public class WeatherReport
    {
        public string City { get; set; }

        public int Temperature { get; set; }
    }
}
