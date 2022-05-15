using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherForecast
    {
        private static string apiKey = "c1c4d772f711221a4a1df5be101bb4a5";
        private static string url1 = "https://api.openweathermap.org/data/2.5/weather?q=";
        //private static string city = String.Empty;
        private static string url2 = "&appid=";
        private static string units = "&units=metric";
        public static async Task <double?> GetTemperature(string city)
        {
            string finalUrl = $"{url1}{city}{url2}{apiKey}{units}";
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(finalUrl);

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
        }
        public static async Task<string> Instructions(double? temperature)
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
