﻿using BL;
using ConsoleApp;

//WeatherForecast weatherForecast = new WeatherForecast(new HttpClient());

var job = DependencyInjection.Resolve<MainJob>();
var mediator = DependencyInjection.Resolve<MainJob>();
await job.Execute();


