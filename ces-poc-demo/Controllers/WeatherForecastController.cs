using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ces_poc_demo.Controllers
{
    [ApiController]
    [Route("/WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecasts")]
        public IActionResult Get()
        {
            return Ok(HttpContext.Session.GetObjectFromJson<List<WeatherForecast>>("forecasts"));
        }

        [HttpPost(Name = "AddWeatherForeCast")]
        public IActionResult Add(WeatherForecast weatherForecast)
        {
            var forecasts = HttpContext.Session.GetObjectFromJson<List<WeatherForecast>>("forecasts");
            if (forecasts != null)
                forecasts.Add(weatherForecast);
            else
                forecasts = new List<WeatherForecast>() { weatherForecast };
            HttpContext.Session.SetObjectAsJson("forecasts", forecasts);

            return Created();
        }

        [HttpPut(Name = "UpdateWeatherForeCast")]
        public IActionResult Update(WeatherForecast weatherForecast)
        {
            var forecasts = HttpContext.Session.GetObjectFromJson<List<WeatherForecast>>("forecasts");
            var forecast = forecasts?.Where(x => x.ID == weatherForecast.ID).FirstOrDefault();

            if (forecast != null)
            {
                forecast.TemperatureC = weatherForecast.TemperatureC;
                forecast.Summary = weatherForecast.Summary;

                forecasts.Where(x => x.ID == weatherForecast.ID).ToList().ForEach(x => x = forecast);
                HttpContext.Session.SetObjectAsJson("forecasts", forecasts);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete(Name = "DeleteWeatherForeCast")]
        public IActionResult Delete(int ID)
        {
            var forecasts = HttpContext.Session.GetObjectFromJson<List<WeatherForecast>>("forecasts");
            var forecast = forecasts?.Where(x => x.ID == ID).FirstOrDefault();

            if (forecast != null)
            {
                forecasts.Remove(forecast);
                HttpContext.Session.SetObjectAsJson("forecasts", forecasts);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
