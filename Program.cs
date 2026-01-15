using FurAndFangs.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Read OpenAI API key
var openAiKey = builder.Configuration["OpenAI:ApiKey"]
               ?? Environment.GetEnvironmentVariable("OPENAI__ApiKey");

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Make enum parsing case-insensitive and use string names
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FurAndFangs API", Version = "v1" });
});

// Add OpenAI HttpClient
if (!string.IsNullOrEmpty(openAiKey))
{
    builder.Services.AddHttpClient("OpenAI", client =>
    {
        client.BaseAddress = new Uri("https://api.openai.com/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiKey);
    });
}

// Add DbContext
builder.Services.AddDbContext<PetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Serve static HTML files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Enable Swagger at /swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FurAndFangs API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
