using FurAndFangs.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Read API key
var openAiKey = builder.Configuration["OpenAI:ApiKey"]
               ?? Environment.GetEnvironmentVariable("OPENAI__ApiKey");

// Add services
builder.Services.AddControllers();
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

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Point to the exact URL where swagger.json is available
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FurAndFangs API v1");

    // Open Swagger UI at root URL
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
