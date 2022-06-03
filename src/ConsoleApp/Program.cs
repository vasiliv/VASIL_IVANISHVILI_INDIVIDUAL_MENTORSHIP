using BL;
using ConsoleApp;

var job = DependencyInjection.Resolve<MainJob>();
await job.Execute();


