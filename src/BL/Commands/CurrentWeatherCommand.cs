using MediatR;
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
        public CurrentWeatherCommandHandler(WeatherForecast weatherForeCast)
        {
            _weatherForecast = weatherForeCast;
        }
        public async Task<double?> Handle(CurrentWeatherCommand request, CancellationToken cancellationToken)
        {            
            Console.WriteLine("Please enter the city:");
            string city = Console.ReadLine();
            double? temperature = await _weatherForecast.GetTemperature(city);
            Console.WriteLine(temperature);
            string instruction = _weatherForecast.Instructions(temperature);
            Console.WriteLine(instruction);
            return temperature;
        }
    }
}
