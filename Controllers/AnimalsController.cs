using Microsoft.AspNetCore.Mvc;
using FurAndFangs.Api.Data;
using FurAndFangs.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
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

        // Get api/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            return await _context.Animals.ToListAsync();
        }

        // Post api/animals/ask
        [HttpPost("ask")]
        public async Task<ActionResult<string>> AskAnimalQuestion([FromBody] string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Please provide a question.");

            var client = _httpClientFactory.CreateClient("OpenAI");

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are an expert pet assistant. Answer questions about pet care, diet, species, and general animal knowledge." },
                    new { role = "user", content = question }
                },
                max_tokens = 200
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
