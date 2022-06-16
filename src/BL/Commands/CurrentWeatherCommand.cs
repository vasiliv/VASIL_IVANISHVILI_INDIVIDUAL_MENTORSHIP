using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Commands
{
    public class CurrentWeatherCommand : IRequest<double?>
    {
        public string City { get; set; }
    }
    public class CurrentWeatherCommandHandler : IRequestHandler<CurrentWeatherCommand, double?>
    {
        private readonly WeatherForecast _weatherForecast;
        private readonly IConfiguration _configuration;
        public CurrentWeatherCommandHandler(WeatherForecast weatherForeCast, IConfiguration configuration)
        {
            _weatherForecast = weatherForeCast;
            _configuration = configuration;
        }
        public async Task<double?> Handle(CurrentWeatherCommand request, CancellationToken cancellationToken)
        {
            //later read from appsettings.json
            using CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            Console.WriteLine("Please enter the city:");
            string city = Console.ReadLine();
            double? temperature = await _weatherForecast.GetTemperature(city, tokenSource.Token);
            Console.WriteLine(temperature);
            string instruction = _weatherForecast.Instructions(temperature);
            Console.WriteLine(instruction);
            return temperature;
        }
    }
}
