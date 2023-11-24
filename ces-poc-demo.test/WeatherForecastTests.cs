using ces_poc_demo.Controllers;
using ces_poc_demo.Implementation;
using ces_poc_demo.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ces_poc_demo.test
{
    public class WeatherForecastTests
    {
        WeatherForecastController oWeather;

        public WeatherForecastTests() 
        {
            List<WeatherForecast> forecasts = [
                new WeatherForecast() { ID = 1, Summary = "Freezing", TemperatureC = 4 },
                new WeatherForecast() { ID = 2, Summary = "Chilly", TemperatureC = 10 },
                new WeatherForecast() { ID = 3, Summary = "Cool", TemperatureC = 15 },
                new WeatherForecast() { ID = 4, Summary = "Warm", TemperatureC = 25 },
                new WeatherForecast() { ID = 5, Summary = "Hot", TemperatureC = 35 }
            ];
            
            MockHttpSession httpcontext = new();
            httpcontext.SetObjectAsJson("forecasts", forecasts);
            oWeather = new WeatherForecastController
            {
                ControllerContext = new ControllerContext 
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            oWeather.HttpContext.Session = httpcontext;
        }

        [Fact]
        public void Get_WeatherForeCasts_Success()
        {
            List<WeatherForecast> forecasts = [
                new WeatherForecast() { ID = 1, Summary = "Freezing", TemperatureC = 4 },
                new WeatherForecast() { ID = 2, Summary = "Chilly", TemperatureC = 10 },
                new WeatherForecast() { ID = 3, Summary = "Cool", TemperatureC = 15 },
                new WeatherForecast() { ID = 4, Summary = "Warm", TemperatureC = 25 },
                new WeatherForecast() { ID = 5, Summary = "Hot", TemperatureC = 35 }
            ];
            MockHttpSession httpcontext = new();
            httpcontext.SetObjectAsJson("forecasts", JsonConvert.SerializeObject(forecasts));
            var result = oWeather.Get();
            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public void Add_WeatherForeCasts_Success()
        {
            WeatherForecast forecast = new()
            {
                ID = 6,
                Summary = "Spring",
                TemperatureC = 20
            };

            var result = oWeather.Add(forecast);
            Assert.Equal(typeof(CreatedResult), result.GetType());
        }

        [Fact]
        public void Update_WeatherForeCasts_Success()
        {
            WeatherForecast forecast = new()
            {
                ID = 5,
                Summary = "Hot",
                TemperatureC = 40
            };

            var result = oWeather.Update(forecast);
            Assert.Equal(typeof(NoContentResult), result.GetType());
        }

        [Fact]
        public void Delete_WeatherForeCasts_Success()
        {
            var result = oWeather.Delete(2);
            Assert.Equal(typeof(NoContentResult), result.GetType());
        }
    }
}