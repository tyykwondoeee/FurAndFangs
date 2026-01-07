using FurAndFangs.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // ? Make sure this is here
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Read API key (user-secrets or environment variable)
var openAiKey = builder.Configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI__ApiKey");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FurAndFangs API", Version = "v1" });
});

if (!string.IsNullOrEmpty(openAiKey))
{
    builder.Services.AddHttpClient("OpenAI", client =>
    {
        client.BaseAddress = new Uri("https://api.openai.com/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiKey);
    });
}

// DbContext
builder.Services.AddDbContext<PetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
