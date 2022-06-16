using BL.Models;
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
        public int success, failed;
        public MaxTemprWeatherCommandHandler(WeatherForecast weatherForeCast, IConfiguration configuration)
        {
            _weatherForecast = weatherForeCast;
            _configuration = configuration;
        }
        public async Task<Unit> Handle(MaxTemprWeatherCommand request, CancellationToken cancellationToken)
        {           
            using CancellationTokenSource tokenSource = new CancellationTokenSource(_configuration.GetValue<int>("timeout"));

            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine("Please enter list of cities:");
            string cities = Console.ReadLine();
            IEnumerable<string> cityArray = _weatherForecast.SplitStringToCityArray(cities);

            List<Task<(string, double?)>> list = new();

            foreach (var city in cityArray)
            {
                try
                {
                    list.Add(Task.FromResult((city, await _weatherForecast.GetTemperature(city, tokenSource.Token))));
                }
                catch (TaskCanceledException)
                {
                   
                }
            }
            success = list.Count;
            failed = cityArray.Count() - success;

            double? maxTemperature = list.Select(t => t.Result)
                .Select(t => t.Item2).Max();            

            watch.Stop();
            Console.WriteLine($"Max temperature from list of cities is: {maxTemperature}, passed {watch.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"Successful requests: {success}, Failed requests: {failed}");
            return Unit.Value;
        }
    }
}
