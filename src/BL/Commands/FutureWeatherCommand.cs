using BL.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Commands
{
    public class FutureWeatherCommand : IRequest<Unit> { }
    public class FutureWeatherCommandHandler : IRequestHandler<FutureWeatherCommand, Unit>
    {
        private readonly WeatherForecast _weatherForecast;
        private readonly IConfiguration _configuration;
        public FutureWeatherCommandHandler(WeatherForecast weatherForeCast, IConfiguration configuration)
        {
            _weatherForecast = weatherForeCast;
            _configuration = configuration;
        }
        public async Task<Unit> Handle(FutureWeatherCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Please enter the city:");            
            Coordinate coordinate = await _weatherForecast.GetCoordinateByCity(Console.ReadLine());
            Console.WriteLine("Please enter number of the days to forecast:");            
            int numDays = int.Parse(Console.ReadLine());            

            await _weatherForecast.GetTemperatureByCoordinatesAndDays(coordinate, numDays);
            
            return Unit.Value;
        }
    }
}