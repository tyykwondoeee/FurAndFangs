using FurAndFangs.Api.Models.Enums;

namespace FurAndFangs.Api.Models
{
    public class AnimalQuestionRequest
    {
        public string Question { get; set; }
        public AnimalType? Species { get; set; } // Optional if unknown
        public Sex? Sex { get; set; } // Optional if unknown
        public DietType? Diet { get; set; } // Optional if unknown
        public double? Weight { get; set; } // Optional if unknown
        public WeightUnit? Unit { get; set; } // Optional if unknown
    }
}
