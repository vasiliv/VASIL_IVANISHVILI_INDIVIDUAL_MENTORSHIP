using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//important because builder.Configure to work!!!
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using System.Configuration;
using Hangfire.Storage;
using BL;
using PersistanceLayer;
using Microsoft.EntityFrameworkCore;
using BackgroundJob.Models;

namespace BackgroundJob
{
    public class Program
    {
        //for using configuration in whole class
        public static IConfiguration ConfFromAppsettings { get; set; }

        public static void Main(string[] args)
        {
            //Load Serilog configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConfFromAppsettings = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(builder =>
                 {
                     builder.Configure(app =>
                     {
                         app.UseRouting();

                         var cities = ConfFromAppsettings.GetSection("Cities").Get<List<Cities>>();
                         //For transfering List of strings to recurrent job
                         List<string> CityName = new List<string>();
                         foreach (var city in cities)
                         {
                             CityName.Add(city.City);
                         }
                         //add recurring job
                         if (cities.ToString().Contains(','))
                         {
                             RecurringJob.AddOrUpdate<BL.WeatherForecast>("Job Id",
                                 x => x.Combination(CityName.ToString()), Cron.Minutely);
                         }
                         else
                         {
                             RecurringJob.AddOrUpdate<BL.WeatherForecast>("Job Id",
                                x => x.FillWeatherList(CityName.ToString()), Cron.Minutely);
                         }
                         //add hangfire dashboard        
                         app.UseHangfireDashboard();
                         app.UseEndpoints(endpoints =>
                 {
                     endpoints.MapHangfireDashboard();
                 });
                     });
                 })
        .ConfigureServices((hostContext, services) =>
        {
            //because of missing this line could not reach Connection string from appsettings.json
            IConfiguration configuration = hostContext.Configuration;
            var cities = configuration.GetSection("Cities").Get<List<Cities>>();
            services.AddSingleton(cities);
            services.AddHostedService<Worker>();
            
            //adding hangfire server
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfireServer();
            services.AddHostedService<Worker>();
            //add DbContext

            services.AddDbContext<WeatherContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        })
        //added
        .UseSerilog();
    }
}
