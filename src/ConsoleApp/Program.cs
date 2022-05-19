using BL;

WeatherForecast weatherForecast = new WeatherForecast(new HttpClient());



Console.WriteLine("Please enter the city:");
string city = Console.ReadLine();
double? temperature = await weatherForecast.GetTemperature(city);
Console.WriteLine(temperature);
string instruction = await weatherForecast.Instructions(temperature);
Console.WriteLine(instruction);