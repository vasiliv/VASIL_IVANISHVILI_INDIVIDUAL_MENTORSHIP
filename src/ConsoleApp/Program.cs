using BL;
using ConsoleApp;

var job = DependencyInjection.Resolve<MainJob>();
var mediator = DependencyInjection.Resolve<MainJob>();
await job.Execute();


