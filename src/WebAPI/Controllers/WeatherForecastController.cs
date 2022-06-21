using BL.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WeatherForecastController(IMediator mediator)
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
    };    
}