using ConsoleAppScheduler.Base;
using ConsoleAppScheduler.DataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ConsoleAppScheduler.Base.Common;

var builder = Host.CreateApplicationBuilder();
var enviroment = Environment.GetEnvironmentVariable(Constants.Environment.ENVIRONMENT_SYSTEM);

var configuration = new ConfigurationBuilder()
 .AddJsonFile($"appsettings.{Constants.Environment.ENV_DEVELOPMENT}.json");
var config = configuration.Build();
await builder.Services.ConfigureJobs(config);
var app = builder.Build();
app.Run();
