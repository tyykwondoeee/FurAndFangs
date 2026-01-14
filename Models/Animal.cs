using FurAndFangs.Api.Models.Enums;

namespace FurAndFangs.Api.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AnimalType Species { get; set; } // Cat,Dog, Reptile, etc.
        public string Breed { get; set; } // Dog, Cat, Rabbit, etc.
        public int Age { get; set; }
        public Sex Sex { get; set; } // Male,Female,Unknown
        public DietType Diet { get; set; } // Herbivore,Carnivore,Omnivore
        public string Notes { get; set; }
        public double Weight { get; set; } // Weight in pounds
        public WeightUnit Unit { get; set; } = WeightUnit.Pounds; // Pounds or Kilograms
    }
}

