using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// important because builder.Configure to work!!!
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
                         var cities = ConfFromAppsettings.GetValue<string>("Cities");
                         //add recurring job
                         if (cities.Contains(','))
                         {
                             RecurringJob.AddOrUpdate<BL.WeatherForecast>("Job Id",
                            x => x.Combination(cities), Cron.Minutely);
                         }
                         else
                         {
                             RecurringJob.AddOrUpdate<BL.WeatherForecast>("Job Id",
                            x => x.FillWeatherList(cities), Cron.Minutely);
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
                    ConfFromAppsettings = configuration;
                    //adding hangfire server                    
                    services.AddHangfire(x =>
                    {
                        x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
                    });
                    services.AddHangfireServer();                    
                    services.AddHostedService<Worker>();
                })
            //added
            .UseSerilog();
    }

}
