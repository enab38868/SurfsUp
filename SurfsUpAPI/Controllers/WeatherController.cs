using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/Weather")]
    public class WeatherController : Controller
    {
       
   
        //public IActionResult Index()
        //{
        //    return View();
            
        //}
        [HttpGet]
        public async Task <WeatherForecast.Root> WeatherByCity(string city)
        {
            
            HttpClient client = new();
            string cityname = city;           
            string api = $"https://api.openweathermap.org/data/2.5/weather?q={cityname}&appid=6fffb45ed1b43a7b98671baa1adc21d3";
            
            var result = await client.GetFromJsonAsync<WeatherForecast.Root>(api);

            return result;

        }

    }
}
