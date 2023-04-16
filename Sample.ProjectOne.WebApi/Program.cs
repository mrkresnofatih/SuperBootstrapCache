using Sample.ProjectOne.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();  // important

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);

app.Run();