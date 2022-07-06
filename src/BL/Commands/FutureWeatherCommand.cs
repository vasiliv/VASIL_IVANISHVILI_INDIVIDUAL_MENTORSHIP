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
    public class FutureWeatherCommand : IRequest<IEnumerable<string>>
    {
        public int NumDays { get; set; }
        public string City { get; set; }
    }    
    public class FutureWeatherCommandHandler : IRequestHandler<FutureWeatherCommand, IEnumerable<string>>
    {
        private readonly WeatherForecast _weatherForecast;
        private readonly IConfiguration _configuration;
        public FutureWeatherCommandHandler(WeatherForecast weatherForeCast, IConfiguration configuration)
        {
            _weatherForecast = weatherForeCast;
            _configuration = configuration;
        }        
        public async Task<IEnumerable<string>> Handle(FutureWeatherCommand request, CancellationToken cancellationToken)
        {
            //get City property from CurrentWeatherCommand - request.City
            Coordinate coordinate = await _weatherForecast.GetCoordinateByCity(request.City);
            //get NumDays property from CurrentWeatherCommand - request.NumDays
            return await _weatherForecast.GetTemperatureByCoordinatesAndDays(coordinate, request.NumDays);            
        }
    }
}