using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FurAndFangs.Api.Data
{
    public class PetContextFactory : IDesignTimeDbContextFactory<PetContext>
    {
        public PetContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PetContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=FurAndFangsDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new PetContext(optionsBuilder.Options);
        }
    }
}
