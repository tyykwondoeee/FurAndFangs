using FurAndFangs.Api.Models;
using FurAndFangs.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FurAndFangs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AnimalsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AnimalQuestionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Request))
                return BadRequest(new { error = "The request field is required." });

            // Build prompt for OpenAI
            var speciesText = request.Species?.ToString() ?? "Unknown";
            var sexText = request.Sex?.ToString() ?? "Unknown";
            var dietText = request.Diet?.ToString() ?? "Unknown";
            var weightText = request.Weight.HasValue && request.Unit.HasValue
                             ? $"{request.Weight} {request.Unit}"
                             : "Unknown";

            var prompt = $@"
You are a helpful assistant for pet owners. 
Answer the question clearly and concisely.

Question: {request.Request}
Species: {speciesText}
Sex: {sexText}
Diet: {dietText}
Weight: {weightText}";

            try
            {
                var client = _httpClientFactory.CreateClient("OpenAI");
                var body = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful pet assistant." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 200
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("v1/chat/completions", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorText);
                }

                var responseText = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseText);
                var answer = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();

                return Ok(answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error contacting OpenAI: {ex.Message}");
            }
        }
    }
}
