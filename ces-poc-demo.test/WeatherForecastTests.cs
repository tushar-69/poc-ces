using AutoFixture;
using AutoFixture.AutoMoq;
using ces_poc_demo.Controllers;
using ces_poc_demo.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ces_poc_demo.test
{
    public class WeatherForecastTests
    {
        private WeatherForecastController _controller;
        private IFixture _fixture;
        private MockHttpSession _httpcontext;

        public WeatherForecastTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            var forecasts = _fixture.CreateMany<WeatherForecast>(3).ToList();
            _httpcontext = new();
            _httpcontext.SetObjectAsJson("forecasts", forecasts);
            _controller = new WeatherForecastController
            {
                ControllerContext = new ControllerContext 
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            _controller.HttpContext.Session = _httpcontext;
        }

        [Fact]
        public void Get_WeatherForeCasts_ReturnsWeatherList()
        {
            var result = _controller.Get();
            
            var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(200);
            var actualValue = objectResult.Value.Should().BeOfType<List<WeatherForecast>>().Subject;
            actualValue.Should().NotBeNull();
            actualValue.Should().HaveCount(3);
        }

        [Fact]
        public void Add_WeatherForeCast_ReturnsCreated()
        {
            WeatherForecast forecast = _fixture.Create<WeatherForecast>();
            
            var result = _controller.Add(forecast);

            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public void Update_WeatherForeCasts_ReturnsNoContent()
        {
            var forecasts = _httpcontext.GetObjectFromJson<List<WeatherForecast>>("forecasts");
            WeatherForecast forecast = forecasts.FirstOrDefault();
            forecast.Summary = "Sunny";

            var result = _controller.Update(forecast);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_WeatherForeCasts_ReturnsNoContent()
        {
            var forecasts = _httpcontext.GetObjectFromJson<List<WeatherForecast>>("forecasts");
            int ID = forecasts.LastOrDefault().ID;

            var result = _controller.Delete(ID);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Add_InvalidTemperature_ReturnsBadRequestValidationError()
        {
            _controller.ModelState.AddModelError("TemperatureC", "The field TemperatureC must be between -270 and 270.");
            _fixture.Customize<WeatherForecast>(x => x.With(x => x.TemperatureC, 1000));
            var forecast = _fixture.Create<WeatherForecast>();

            var result = _controller.Add(forecast);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Add_InvalidSummary_ReturnsBadRequestValidationError()
        {
            _controller.ModelState.AddModelError("Summary", "The field Summary must be a string or array type with a minimum length of '3'.");
            _fixture.Customize<WeatherForecast>(x => x.With(x => x.Summary, "HT"));
            var forecast = _fixture.Create<WeatherForecast>();

            var result = _controller.Add(forecast);

            var actualValue = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            actualValue.StatusCode.Should().Be(400);
            ((ModelStateDictionary.ValueEnumerable)actualValue.Value).AsEnumerable().ToList()[0].Errors[0].ErrorMessage.Should().Be("The field Summary must be a string or array type with a minimum length of '3'.");            
        }

        [Fact]
        public void Update_InvalidWeatherForecast_ReturnsNotFound()
        {
            var forecast = _fixture.Create<WeatherForecast>();

            var result = _controller.Update(forecast);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFound()
        {
            var forecastID = _fixture.Create<WeatherForecast>().ID;

            var result = _controller.Delete(forecastID);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}