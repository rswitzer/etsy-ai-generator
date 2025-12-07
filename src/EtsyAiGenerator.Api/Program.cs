using System.Text.Json;
using System.Text.Json.Serialization;
using EtsyAiGenerator.Application.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Application is responding normally."));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();

var healthCheckOptions = new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            durationMs = report.TotalDuration.TotalMilliseconds,
            timestamp = DateTimeOffset.UtcNow,
            details = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                durationMs = entry.Value.Duration.TotalMilliseconds
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
};

app.MapControllers();

app.Map("/error", (HttpContext httpContext) => Results.Problem())
    .ExcludeFromDescription();

app.MapGet("/health", () =>
{
    var now = DateTimeOffset.UtcNow;
    const string status = "Healthy";

    var html = $"""
        <!doctype html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
            <title>Etsy AI Generator | Health</title>
        </head>
        <body style="margin:0;min-height:100vh;display:grid;place-items:center;font-family:'Segoe UI',Tahoma,sans-serif;color:#0f172a;background:linear-gradient(135deg,#eef2ff,#ecfeff)">
            <main style="padding:2.5rem 3rem;border-radius:1.25rem;background:#ffffffcc;box-shadow:0 25px 45px rgba(15,23,42,0.15);backdrop-filter:blur(6px);text-align:center">
                <h1 style="margin-bottom:0.5rem;font-size:2rem">Service status: {status}</h1>
                <p style="margin:0.25rem 0">Environment: {app.Environment.EnvironmentName}</p>
                <p style="margin:0.25rem 0">Checked at: {now:O}</p>
                <a style="display:inline-block;margin-top:1.25rem;color:#2563eb" href="/health/status" rel="nofollow">View JSON diagnostics</a>
            </main>
        </body>
        </html>
        """;

    return Results.Content(html, "text/html");
});

app.MapHealthChecks("/health/status", healthCheckOptions);

app.Run();
