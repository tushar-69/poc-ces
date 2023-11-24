using System.ComponentModel.DataAnnotations;

namespace ces_poc_demo
{
    public class WeatherForecast
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(-270, 270)]
        public int TemperatureC { get; set; }

        [Required]
        [MinLength(3)]
        public string? Summary { get; set; }
    }
}
