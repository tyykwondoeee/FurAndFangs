using Microsoft.EntityFrameworkCore;
using FurAndFangs.Api.Models;

namespace FurAndFangs.Api.Data
{
    public class PetContext : DbContext
    {
        public PetContext(DbContextOptions<PetContext> options)
            : base(options)
        {
        }

            public DbSet<Animal> Animals { get; set; }
    }
}
