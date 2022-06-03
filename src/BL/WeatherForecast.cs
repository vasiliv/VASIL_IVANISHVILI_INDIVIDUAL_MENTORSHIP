using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherForecast
    {
        private readonly HttpClient _httpClient;        
        public WeatherForecast(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }        
        public async Task <double?> GetTemperature(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=c1c4d772f711221a4a1df5be101bb4a5&units=metric";
                HttpResponseMessage result = await _httpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    JObject obj = JsonConvert.DeserializeObject<JObject>(json);
                    JObject mainObj = obj["main"] as JObject;
                    double temperature = (double)mainObj["temp"];
                    return temperature;
                }
                else
                {
                    return null;
                }
        }
        public string Instructions(double? temperature)
        {
            return temperature switch
            {
                < 0 => "Dress warmly",
                >= 0 and < 20 => "It's fresh",
                >= 20 and < 30 => "Good weather",
                >= 30 => "It's time to go to the beach",
                _ => "No such a temperature"
            };
        }        
    }        
}

