using Dapper;
using Waybon.Api;
using Waybon.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
DefaultTypeMap.MatchNamesWithUnderscores = true;

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add layers
builder.Services.AddApiServices(builder.Configuration);

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.MapHub<LocationHub>("/hubs/location");

app.Run();