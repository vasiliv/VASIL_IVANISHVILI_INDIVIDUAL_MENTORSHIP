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
            Console.WriteLine("Please enter the number:\n1 - One day forecast\n2 - Several day forecast\n3 - Several cities forecast\n0 - Exit ");
            int choice = Int16.Parse(Console.ReadLine());
            await NumberChoice(choice);            
        }
        //Choices only for console app
        public async Task NumberChoice(int choice)
        {            
            switch(choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Console.WriteLine("Please enter the city:");
                    string city = Console.ReadLine();                    
                    
                    //passing value to CurrentWeatherCommand City property
                    await _mediator.Send(new CurrentWeatherCommand { 
                        City = city
                    });
                    break;
                case 2:
                    Console.WriteLine("Please enter the city:");
                    //city2 because of case 2
                    string city2 = Console.ReadLine();
                    Console.WriteLine("Please enter number of the days to forecast:");
                    int numDays = int.Parse(Console.ReadLine());

                    //passing values to FutureWeatherCommand City and NumDays property
                    await _mediator.Send(new FutureWeatherCommand
                    {
                        City = city2,
                        NumDays = numDays
                    });                    
                    break;
                case 3:
                    await _mediator.Send(new MaxTemprWeatherCommand());
                    break;
                default:
                    Console.WriteLine("Please enter correct number");
                    break;
            };
        }
    }
}
