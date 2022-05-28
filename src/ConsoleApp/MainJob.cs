using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class MainJob
    {
        private readonly WeatherForecast _weatherForecast;

        public MainJob(WeatherForecast weatherForeCast)
        {
            _weatherForecast = weatherForeCast;
        }
        public async Task Execute()
        {    

            Console.WriteLine("Please enter the city:");
            string city = Console.ReadLine();
            double? temperature = await _weatherForecast.GetTemperature(city);
            Console.WriteLine(temperature);
            string instruction = _weatherForecast.Instructions(temperature);
            Console.WriteLine(instruction);            
        }
    }
}
