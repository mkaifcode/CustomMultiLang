using Microsoft.AspNetCore.Mvc;
using MultiLangApi.Helpers.Response;

namespace MultiLangApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ResponseHelper _responseHelper;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ResponseHelper responseHelper)
        {
            _logger = logger;
            _responseHelper = responseHelper;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("test-response")]
        public IActionResult TestResponse()
        {
            var response = _responseHelper.CreateResponse("WelcomeMessage", new { home = "jome", titel = "Tital" }, 200);
            return _responseHelper.ResponseWrapper(response);
        }

    }
}
