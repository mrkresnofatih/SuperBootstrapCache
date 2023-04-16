# DesertCamel.BaseMicroservices.SuperBootstrap.Cache

## Get Started

1. Setup a .NET WebAPI with SuperBootstrapBase setup.
2. Install SuperBootstrapCache NuGet Package.
3. Add the cache section inside your `appsettings.json` file.
```json
{
    // other settings
    "SuperBootstrap": {
        // other superbootstrap settings
        "Cache": {
            "Selected": "Redis",
            "Options": {
                "Redis": {
                    "ConnectionString": "localhost:6379,password=myredispassword,abortConnect=false",
                    "ServiceName": "ProjectOneWebApi"
                }
            }
        }
    }
}
```

4. Add the extension method for SuperBootstrapCache in your `Startup.cs` file:
```c#
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // other services

            services.AddBootstrapBase(Configuration);
            services.AddBootstrapCache(Configuration); // add this
        }

        // other
    }
}
```

5. Use the ISuperBootstrapCache utility as you like (already implements dependency injection)

```c#
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
```