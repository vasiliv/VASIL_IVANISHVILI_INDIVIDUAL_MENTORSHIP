using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Commands
{
    public class MaxTemprWeatherCommand: IRequest<Unit>
    {
    }
    public class MaxTemprWeatherCommandHandler : IRequestHandler<MaxTemprWeatherCommand, Unit>
    {
        private readonly WeatherForecast _weatherForecast;
        private readonly IConfiguration _configuration;
        public MaxTemprWeatherCommandHandler(WeatherForecast weatherForeCast, IConfiguration configuration)
        {
            _weatherForecast = weatherForeCast;
            _configuration = configuration;
        }
        public async Task<Unit> Handle(MaxTemprWeatherCommand request, CancellationToken cancellationToken)
        {
            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine("Please enter list of cities:");
            string cities = Console.ReadLine();
            IEnumerable<string> cityArray = _weatherForecast.SplitStringToCityArray(cities);            

            var tasks = new List<Task<double?>>();
            foreach (var city in cityArray)
            {
                tasks.Add(_weatherForecast.GetTemperature(city));
            }
            double?[] temperatures = await Task.WhenAll(tasks);
            double? maxTemperature = temperatures.Max();
            watch.Stop();
            Console.WriteLine($"Max temperature from list of cities is: {maxTemperature.ToString()}, passed {watch.ElapsedMilliseconds} milliseconds");
            
            return Unit.Value;
        }
    }
}
