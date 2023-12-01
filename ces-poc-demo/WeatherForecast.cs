using System.ComponentModel.DataAnnotations;

namespace ces_poc_demo
{
    public class WeatherForecast
    {
        [Key]
        public int ID { get; set; }

        [Range(-270, 270)]
        public int TemperatureC { get; set; }

        [MinLength(3)]
        public string Summary { get; set; }
    }
}
