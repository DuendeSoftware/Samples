using System;
using System.Collections;
using System.Collections.Generic;

namespace BlazorServer.Data
{
    public class Weather
    {
        public string User { get; set; }

        public IEnumerable<WeatherForecast> Forecast { get; set; }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}