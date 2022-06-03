using BL;
using BL.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class DependencyInjection
    {
        public static T Resolve <T>()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            return new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<MainJob>()
                .AddSingleton<WeatherForecast>()
                .AddHttpClient()
                .AddMediatR(typeof(CurrentWeatherCommand).Assembly)
                .BuildServiceProvider()
                .GetRequiredService<T>();
        }
    }
}
