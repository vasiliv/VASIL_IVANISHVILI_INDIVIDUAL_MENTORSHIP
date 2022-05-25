using BL;
using System.Configuration;

WeatherForecast weatherForecast = new WeatherForecast(new HttpClient());

int minDate = Int32.Parse(ConfigurationManager.AppSettings["minDate"]);
int maxDate = Int32.Parse(ConfigurationManager.AppSettings["maxDate"]);

Console.WriteLine("Please enter the city:");
string city = Console.ReadLine();
double? temperature = await weatherForecast.GetTemperature(city);
Console.WriteLine(temperature);
string instruction = await weatherForecast.Instructions(temperature);
Console.WriteLine(instruction);