var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// API routes
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

if (app.Environment.IsDevelopment())
{
    // Catch static file requests and proxy them too
    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value ?? "";
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, path);
        
        // Let API requests through
        if (path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/openapi/", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }
        
        // Proxy everything else to Angular
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var angularDevServer = "http://localhost:4200";
        var targetUri = $"{angularDevServer}{path}{context.Request.QueryString}";
        
        logger.LogInformation("Proxying to: {TargetUri}", targetUri);
        
        try
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = new Uri(targetUri)
            };
            
            foreach (var header in context.Request.Headers)
            {
                if (!header.Key.StartsWith(":", StringComparison.OrdinalIgnoreCase))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
            
            var responseMessage = await httpClient.SendAsync(requestMessage, 
                HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            
            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            
            context.Response.Headers.Remove("transfer-encoding");
            
            await responseMessage.Content.CopyToAsync(context.Response.Body);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error proxying request");
            context.Response.StatusCode = 502;
            await context.Response.WriteAsync("Angular dev server not running");
        }
    });
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}