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

namespace BackgroundJob
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; }
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

                         //adding hangfire dashboard
                         app.UseHangfireDashboard();
                         app.UseEndpoints(endpoints =>
                         {
                             endpoints.MapHangfireDashboard();
                         });
                     });
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    //adding hangfire server                    
                    //services.AddHangfire(conf =>
                    //conf.UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BackgroundJob;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
                    //services.AddSingleton<IConfiguration>(Configuration);
                    services.AddHangfire(x =>
                    {
                        x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
                    });
                    services.AddHangfireServer();                    
                    services.AddHostedService<Worker>();
                })
            //added
            .UseSerilog(); 
    }
}
