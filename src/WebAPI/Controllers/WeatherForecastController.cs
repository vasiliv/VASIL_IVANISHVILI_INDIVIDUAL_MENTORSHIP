using BL.Commands;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(IMediator mediator,
                IRecurringJobManager recurringJobManager,
                ILogger<WeatherForecastController> logger)
        {
            _mediator = mediator;
        }
        [HttpGet("currentWeather/{city}")]
        public async Task<double?> GetCurrentWeather([FromRoute] string city)
        {                      
            return await _mediator.Send(new CurrentWeatherCommand
            {
                City = city
            });            
        }
        [HttpGet("currentWeather/{city}/days/{numDays}")]
        public async Task <IEnumerable<string>> GetFutureWeather([FromRoute] string city, [FromRoute] int numDays)
        {            
            return await _mediator.Send(new FutureWeatherCommand
                {
                    City = city,
                    NumDays = numDays
            });
        }
    };    
}