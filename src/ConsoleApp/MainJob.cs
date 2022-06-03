using BL;
using BL.Commands;
using MediatR;
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
        private readonly IMediator _mediator;
        public MainJob(WeatherForecast weatherForeCast, IMediator mediator)
        {
            _weatherForecast = weatherForeCast;
            _mediator = mediator;
        }
        public async Task Execute()
        {
            Console.WriteLine("Please enter the number:");
            int choice = Int16.Parse(Console.ReadLine());
            await NumberChoice(choice);            
        }
        public async Task NumberChoice(int choice)
        {            
            switch(choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await _mediator.Send(new CurrentWeatherCommand());
                    break;
                case 2:
                    //go to FutureWeatherCommand,
                    break;
                default:
                    Console.WriteLine("Please enter correct number");
                    break;
            };
        }
    }
}
