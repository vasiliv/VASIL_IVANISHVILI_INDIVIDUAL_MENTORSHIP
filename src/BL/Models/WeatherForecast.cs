using BL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherForecast
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public WeatherForecast(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<double?> GetTemperature(string city)
        {            
            string url = $"{_configuration.GetSection("urlForGetTemperaturePart1").Value}{city}{_configuration.GetSection("urlForGetTemperaturePart2").Value}";
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
        public async Task<Coordinate> GetCoordinateByCity(string name)
        {            
            string url = $"{_configuration.GetSection("urlForGetCoordinateByCityPart1").Value}{name}{_configuration.GetSection("urlForGetCoordinateByCityPart2").Value}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                JObject[] obj = JsonConvert.DeserializeObject<JObject[]>(json);
                if (obj.Length > 0)
                {
                    return new Coordinate
                    {
                        Latitude = (double)obj[0]["lat"],
                        Longitude = (double)obj[0]["lon"]
                    };
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<IEnumerable<double>> GetTemperatureByCoordinatesAndDays(Coordinate coordinate, int numDays)
        {            
            string url = $"{_configuration.GetSection("urlForGetTemperatureByCoordinatesAndDaysPart1").Value}{coordinate.Latitude}&lon={coordinate.Longitude}{_configuration.GetSection("urlForGetTemperatureByCoordinatesAndDaysPart2").Value}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);
            double[] temperature = new double[numDays];

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var deserializedJson = JsonConvert.DeserializeObject<JToken>(json);
                var daily = deserializedJson["daily"];
                
                for (int i = 0; i < numDays; i++)
                {                    
                    temperature[i] = (double)daily[i]["temp"]["day"];                    
                    Console.WriteLine($"Day {i+1} temperature {temperature[i]} {Instructions(temperature[i])}");
                }
            }
            return temperature;
        }
    }   
}

