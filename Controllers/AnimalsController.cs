using FurAndFangs.Api.Models;
using FurAndFangs.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FurAndFangs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        [HttpPost("ask")]
        public IActionResult Ask([FromBody] AnimalQuestionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Request))
            {
                return BadRequest("The request field is required.");
            }

            var response =
$@"You asked: {request.Request}
Species: {request.Species?.ToString() ?? "Not specified"}
Sex: {request.Sex?.ToString() ?? "Not specified"}
Diet: {request.Diet?.ToString() ?? "Not specified"}
Weight: {(request.Weight.HasValue ? request.Weight.ToString() : "Not specified")}
{(request.Unit.HasValue ? request.Unit.ToString() : "")}

(Mock response — OpenAI is disabled)";

            return Ok(response);
        }
    }
}
