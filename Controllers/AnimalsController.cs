using Microsoft.AspNetCore.Mvc;
using FurAndFangs.Api.Data;
using FurAndFangs.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FurAndFangs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly PetContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnimalsController(PetContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            return await _context.Animals.ToListAsync();
        }

        // POST: api/animals/ask
        [HttpPost("ask")]
        public async Task<ActionResult<string>> AskAnimalQuestion([FromBody] AnimalQuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Please ask a question.");

            var client = _httpClientFactory.CreateClient("OpenAI");

            // Build prompt with context
            var systemPrompt = "You are an expert pet assistant. Answer questions about pet care, diet, species, and general animal knowledge." +
                "Include a disclaimer in every response that this advice is general information and that users should consult a licensed vet for medical concerns.";

            if (request.Species != null)
                systemPrompt += $" The animal is a {request.Species}.";
            if (request.Sex != null)
                systemPrompt += $" Sex: {request.Sex}.";
            if (request.Diet != null)
                systemPrompt += $" Diet: {request.Diet}.";
            if (request.Weight != null && request.Unit != null)
                systemPrompt += $" Weight: {request.Weight} {request.Unit}.";

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = request.Question }
                },
                max_tokens = 400
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "AI request failed.");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);
            var answer = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString()?.Trim();

            return Ok(answer);
        }
    }
}
