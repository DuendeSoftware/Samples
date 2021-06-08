using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorServer.Data
{
    public class WeatherForecastService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<Weather> GetForecastAsync(DateTime startDate)
        {
            var weather = new Weather();
            
            var context = _httpContextAccessor.HttpContext;
            
            if (!context.User.Identity.IsAuthenticated)
            {
                weather.User = "anonymous";
            }
            else
            {
                var client = _httpClientFactory.CreateClient("api_client");
                var userName = await client.GetStringAsync("identity");

                weather.User = userName;
            }
            
            var rng = new Random();
            weather.Forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();

            return weather;
        }
    }
}
