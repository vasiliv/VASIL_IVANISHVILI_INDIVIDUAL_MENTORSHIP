using BL;
using ConsoleApp;

//WeatherForecast weatherForecast = new WeatherForecast(new HttpClient());

var job = DependencyInjection.ServiceCollectionDI<MainJob>();
await job.Execute();


