﻿using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SurfsUpAPI.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
            
        }
        public async Task <List<WeatherForecast.Root>> WeatherByCity()
        {
            HttpClient client = new();
            string cityname = "";
            string api = $"https://api.openweathermap.org/data/2.5/weather?q={cityname}&appid=6fffb45ed1b43a7b98671baa1adc21d3";
            var result = client.GetFromJsonAsync<WeatherForecast.Root>(api);

            return (result);

        }

    }
}
