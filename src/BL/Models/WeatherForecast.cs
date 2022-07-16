using DAL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        //overloaded version of GetTemperature(string city, CancellationToken token)
        public Task<double?> GetTemperature(string city) => GetTemperature(city, CancellationToken.None);
        public async Task<double?> GetTemperature(string city, CancellationToken token)
        {            
            string url = $"{_configuration.GetValue<string>("urlForGetTemperaturePart1")}{city}{_configuration.GetValue<string>("urlForGetTemperaturePart2")}";
            HttpResponseMessage result = await _httpClient.GetAsync(url, token);
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
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
            string url = $"{_configuration.GetValue<string>("urlForGetCoordinateByCityPart1")}{name}{_configuration.GetValue<string>("urlForGetCoordinateByCityPart2")}";
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
        public async Task<IEnumerable<string>> GetTemperatureByCoordinatesAndDays(Coordinate coordinate, int numDays)
        {            
            string url = $"{_configuration.GetValue<string>("urlForGetTemperatureByCoordinatesAndDaysPart1")}{coordinate.Latitude}&lon={coordinate.Longitude}{_configuration.GetValue<string>("urlForGetTemperatureByCoordinatesAndDaysPart2")}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);
            double[] temperature = new double[numDays];
            string[] output = new string[numDays];

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var deserializedJson = JsonConvert.DeserializeObject<JToken>(json);
                var daily = deserializedJson["daily"];
                
                for (int i = 0; i < numDays; i++)
                {                    
                    temperature[i] = (double)daily[i]["temp"]["day"];
                    Console.WriteLine($"Day {i+1} temperature {temperature[i]} {Instructions(temperature[i])}");
                    output[i] = $"Day {i + 1} temperature {temperature[i]} {Instructions(temperature[i])}";
                }
            }            
            return output;
        }
        public IEnumerable<string> SplitStringToCityArray(string cities)
        {
            return cities.Split(',').ToList<string>();
        }
        
        List<Weather> WeatherObj = new List<Weather>() { };
        public async Task FillWeatherList(string city)
        {
            var weather = new Weather()
            {
                City = city,                
                Temperature = await GetTemperature(city),                
            };
            WeatherObj.Add(weather);
        }        
        public async Task Combination(string cities)
        {
            var cityList = SplitStringToCityArray(cities);
            foreach (var item in cityList)
            {
                await FillWeatherList(item);                
            }
        }
    }   
}

