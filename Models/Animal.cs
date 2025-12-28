using System;

namespace FurAndFangs.Api.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; } // Cat,Dog, Reptile, etc.
        public string Breed { get; set; } // Dog, Cat, Rabbit, etc.
        public int Age { get; set; }
        public string Sex { get; set; } // Male,Female,Unknown
        public string Diet { get; set; } // Herbivore,Carnivore,Omnivore
        public string Notes { get; set; }
    }
}

