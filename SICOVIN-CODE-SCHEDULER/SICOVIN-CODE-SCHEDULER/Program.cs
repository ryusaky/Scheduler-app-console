using Microsoft.EntityFrameworkCore;
using Quartz;
using SICOVIN_CODE_SCHEDULER.Base;
using SICOVIN_CODE_SCHEDULER.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SICOVINDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SICOVINDBCnx"));
    }
);
//var host = Host.CreateDefaultBuilder(args).Build();

await builder.Services.ConfigureJobs();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
