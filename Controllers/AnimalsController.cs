using Microsoft.AspNetCore.Mvc;
using FurAndFangs.Api.Data;
using FurAndFangs.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FurAndFangs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly PetContext _context;
        public AnimalsController(PetContext context)
        {
            _context = context;
        }
        // Get apt/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            return await _context.Animals.ToListAsync();
        }
        // post api/animals
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimals), new { id = animal.Id }, animal);
        }
    }
}
