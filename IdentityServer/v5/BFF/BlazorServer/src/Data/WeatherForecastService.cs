using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlazorServer.Data
{
    public class WeatherForecastService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var context = _httpContextAccessor.HttpContext;
            
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
