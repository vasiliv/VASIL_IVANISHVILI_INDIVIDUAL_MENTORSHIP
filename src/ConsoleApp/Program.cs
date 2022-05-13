using BL;

Console.WriteLine("Please enter the city:");
string city = Console.ReadLine();
float temperature = await WeatherForecast.GetTemperature(city);
Console.WriteLine(temperature);
string instruction = await WeatherForecast.Instructions(temperature);
Console.WriteLine(instruction);