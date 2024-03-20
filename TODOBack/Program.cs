using Microsoft.EntityFrameworkCore;
using TODOBack.Data;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("PGHOST");
var database = Environment.GetEnvironmentVariable("PGDATABASE");
var user = Environment.GetEnvironmentVariable("PGUSER");
var password = Environment.GetEnvironmentVariable("PGPASSWORD");
var pgPort = Environment.GetEnvironmentVariable("PGPORT");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql($"Host={host};Port={pgPort};Database={database};Username={user};Password={password}"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}

app.UseAuthorization();

app.MapControllers();

app.Run();