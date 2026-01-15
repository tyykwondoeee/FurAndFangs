using FurAndFangs.Api.Models.Enums;

namespace FurAndFangs.Api.Models
{
    public class AnimalQuestionRequest
    {
        public string Request { get; set; }
        public AnimalType? Species { get; set; }
        public Sex? Sex { get; set; }
        public DietType? Diet { get; set; }
        public double? Weight { get; set; }
        public WeightUnit? Unit { get; set; }
    }
}
