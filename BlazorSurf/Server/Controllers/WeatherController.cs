using BlazorSurf.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSurf.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public async Task<WeatherForecast.Root> WeatherByCity(string city)
        {
            HttpClient client = new();
            string api = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid=6fffb45ed1b43a7b98671baa1adc21d3&units=metric";

            var result = await client.GetFromJsonAsync<WeatherForecast.Root>(api);

            return result;
        }
    }
}
